﻿using System.Collections.Generic;
using Butterfly.HabboHotel.Items;
using Butterfly.HabboHotel.Rooms.Games;
using Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Interfaces;
using System.Collections;
using Database_Manager.Database.Session_Details.Interfaces;
using System.Data;
using System;
using Butterfly.HabboHotel.Items.Interactors;
using Butterfly.Util;

namespace Butterfly.HabboHotel.Rooms.Wired.WiredHandlers.Triggers
{
    class WalksOnFurni : IWiredTrigger
    {
        private RoomItem item;
        private WiredHandler handler;
        private List<RoomItem> items;
        private readonly UserWalksFurniDelegate delegateFunction;

        public WalksOnFurni(RoomItem item, WiredHandler handler, List<RoomItem> targetItems)
        {
            this.item = item;
            this.handler = handler;
            items = targetItems;
            delegateFunction = targetItem_OnUserWalksOnFurni;
            
            foreach (var targetItem in targetItems)
            {
                targetItem.OnUserWalksOnFurni += delegateFunction;
            }
        }

        public List<RoomItem> Items
        {
            get { return items; }
        }

        private void targetItem_OnUserWalksOnFurni(object sender, UserWalksOnArgs e)
        {
            RoomItem onItem = (RoomItem)sender;
            if (onItem.GetBaseItem().IsSeat)
            {
                if (onItem.TotalHeight - onItem.GetBaseItem().Height != e.user.Z)
                    return;
            }
            else
            {
                if (onItem.TotalHeight != e.user.Z)
                    return;
            }

            handler.RequestStackHandle(item, (RoomItem)sender, e.user, e.user.team);
            //InteractorGenericSwitch.DoAnimation(item);
        }

        public void Dispose()
        {
            if (items != null)
            {
                foreach (var targetItem in items)
                {
                    targetItem.OnUserWalksOnFurni -= delegateFunction;
                }
                items.Clear();
            }
            items = null;
            item = null;
            handler = null;
        }

        public void SaveToDatabase(QueryChunk wiredInserts)
        {
            string wired_data = ";;false";
            string wired_to_item = "";
            if (items.Count > 0)
            {
                lock (items)
                {
                    foreach (var id in items)
                    {
                        wired_to_item += id.Id + ";";
                    }
                    if (wired_to_item.Length > 0)
                        wired_to_item = wired_to_item.Substring(0, wired_to_item.Length - 1);
                }
            }
            string wired_original_location = "";

            wiredInserts.AddQuery("('" + item.Id + "', @data" + item.Id + ", @to_item" + item.Id + ", @original_location" + item.Id + ")");
            wiredInserts.AddParameter("data" + item.Id, wired_data);
            wiredInserts.AddParameter("to_item" + item.Id, wired_to_item);
            wiredInserts.AddParameter("original_location" + item.Id, wired_original_location);
        }
    }
}
