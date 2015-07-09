//
// RozWorld.Network.Chat.PermissionGroup -- RozWorld Permission Group
//
// This source-code is part of the RozWorld project by rozza of Oddmatics:
// <<http://www.oddmatics.co.uk>>
// <<http://www.oddmatics.co.uk/projects/rozworld>>
//
// Sharing, editing and general licence term information can be found inside of the "sup.txt" file that should be located in the root of this project's directory structure.
//

using System.Collections.Generic;

namespace RozWorld.Network.Chat
{
    public class PermissionGroup
    {
        private List<string> Permissions;
        public string Prefix;


        public PermissionGroup(string prefix, List<string> permissions)
        {
            Prefix = prefix;
            Permissions = permissions;
        }


        /// <summary>
        /// Check whether this group has the specified permission.
        /// </summary>
        /// <param name="permission">The permission tag.</param>
        /// <returns>Whether this group has the specified permission or not.</returns>
        public bool HasPermission(string permission)
        {
            bool permissionExists = false;
            int i = 0;

            do
            {
                if (Permissions[i] == permission.ToLower())
                {
                    permissionExists = true;
                }

                i++;
            } while (i <= Permissions.Count - 1 && !permissionExists);

            return permissionExists;
        }


        /// <summary>
        /// Adds a permission to this group.
        /// </summary>
        /// <param name="permission">The permission tag.</param>
        /// <returns>Whether the permission was successfully added or not.</returns>
        public bool AddPermission(string permission)
        {
            if (!HasPermission(permission))
            {
                Permissions.Add(permission.ToLower());
                return true;
            }

            return false;
        }


        /// <summary>
        /// Removes a permission from this group.
        /// </summary>
        /// <param name="permission">The permission tag.</param>
        /// <returns>Whether the permission was successfully removed or not.</returns>
        public bool RemovePermission(string permission)
        {
            if (HasPermission(permission))
            {
                Permissions.Remove(permission.ToLower());
                return true;
            }

            return false;
        }
    }
}
