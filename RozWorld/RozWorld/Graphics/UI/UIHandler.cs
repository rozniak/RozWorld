/**
 * RozWorld.Graphics.UI.UIHandler -- RozWorld UI Handler
 *
 * This source-code is part of the RozWorld project by rozza of Oddmatics:
 * <<http://www.oddmatics.co.uk>>
 * <<http://roz.world>>
 * <<http://github.com/rozniak/RozWorld>>
 *
 * Sharing, editing and general licence term information can be found inside of the "LICENCE.MD" file that should be located in the root of this project's directory structure.
 */

using RozWorld.Graphics.UI.Geometry;
using RozWorld.Graphics.UI.Strings;

using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace RozWorld.Graphics.UI
{
    public class UIHandler
    {
        public Tuple<string, int>[] ControlZMap
        {
            get;
            private set;
        }

        public Dictionary<string, ControlSkeleton> Controls = new Dictionary<string, ControlSkeleton>();
        public Dictionary<string, ControlSystem> ControlSystems = new Dictionary<string, ControlSystem>();

        public GUIOMETRY Geometry = new GUIOMETRY();
        public LanguageSystem Language = new LanguageSystem();


        /// <summary>
        /// Perform a sort to arrange controls in order of Z-Index.
        /// </summary>
        public void SortControlZIndexes()
        {
            int indexSetup = 0;
            ControlZMap = new Tuple<string, int>[Controls.Count];

            foreach (var item in Controls)
            {
                ControlZMap[indexSetup] = new Tuple<string, int>(item.Key, item.Value.ZIndex);
                indexSetup++;
            }

            ControlZMap = MergeSortZMap(ControlZMap);
        }


        /// <summary>
        /// Implementation of splitting the array in the merge sort, based off pseudocode on Wikipedia (this would be MergeSort(m)).
        /// </summary>
        /// <param name="map">The Z-Index map array.</param>
        /// <returns>The sorted array.</returns>
        private Tuple<string, int>[] MergeSortZMap(Tuple<string, int>[] map)
        {
            if (map.Length <= 1) { return map; }

            int middle = map.Length / 2;
            Tuple<string, int>[] left = new Tuple<string, int>[middle];
            Tuple<string, int>[] right = new Tuple<string, int>[map.Length - middle];

            for (int i = 0; i < middle; i++)
            {
                left[i] = map[i];
            }

            for (int i = middle; i <= map.Length - 1; i++)
            {
                right[i - middle] = map[i];
            }

            left = MergeSortZMap(left);
            right = MergeSortZMap(right);

            return MergeZMap(left, right);
        }


        /// <summary>
        /// Implementation of comparing and merging the left and right sub-arrays, based off pseudocode on Wikipedia (this would be Merge(left, right)).
        /// </summary>
        /// <param name="left">The left sub-array.</param>
        /// <param name="right">The right sub-array.</param>
        /// <returns>The compared and merged left and right sub-arrays in one array.</returns>
        private Tuple<string, int>[] MergeZMap(Tuple<string, int>[] left, Tuple<string, int>[] right)
        {
            Tuple<string, int>[] result = new Tuple<string, int>[left.Length + right.Length];
            Tuple<string, int>[] buffer = new Tuple<string, int>[0]; // Array to hold values temporarily when shrinking left or right during appending to result.
            int appendingIndex = 0; // Keep track of the next index that will be appended to in result.

            do
            {
                if (left[0].Item2 <= right[0].Item2)
                {
                    result[appendingIndex] = left[0];
                    appendingIndex++;

                    if (left.Length == 1)
                    {
                        left = null;
                    }
                    else
                    {
                        buffer = new Tuple<string, int>[left.Length - 1];
                        Array.Copy(left, 1, buffer, 0, left.Length - 1);

                        left = new Tuple<string, int>[buffer.Length];
                        Array.Copy(buffer, left, buffer.Length);
                    }
                }
                else
                {
                    result[appendingIndex] = right[0];
                    appendingIndex++;

                    if (right.Length == 1)
                    {
                        right = null;
                    }
                    else
                    {
                        buffer = new Tuple<string, int>[right.Length - 1];
                        Array.Copy(right, 1, buffer, 0, right.Length - 1);

                        right = new Tuple<string, int>[buffer.Length];
                        Array.Copy(buffer, right, buffer.Length);
                    }
                }
            } while (left != null && right != null);

            if (left != null)
            {
                Array.Copy(left, 0, result, appendingIndex, left.Length);
            }
            else
            {
                Array.Copy(right, 0, result, appendingIndex, right.Length);
            }

            return result;
        }


        /// <summary>
        /// Destroys all controls related to the specified dialog key.
        /// </summary>
        /// <param name="dialogKey">The dialog key owning the controls.</param>
        public void KillFromDialogKey(int dialogKey)
        {
            List<string> keysToKill = new List<string>();

            foreach (var item in Controls)
            {
                if (item.Value.DialogKey == dialogKey)
                {
                    keysToKill.Add(item.Key);
                }
            }

            foreach (string key in keysToKill)
            {
                Controls.Remove(key);
            }
        }


        /// <summary>
        /// Alerts the user of a critical error and closes the game.
        /// </summary>
        /// <param name="errorCode">The error code to handle.</param>
        public static void CriticalError(short errorCode, string details = null)
        {
            string detailsProvided = String.Empty;

            if(details != null)
            {
                detailsProvided = "\n\nDetails provided:\n" + details;
            }

            switch (errorCode)
            {
                case Error.MISSING_CRITICAL_FILES:
                    MessageBox.Show("RozWorld failed to find critical game assets necessary to run." + detailsProvided + "\n\nRozWorld will now exit.",
                        "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case Error.BROKEN_GUIOMETRY_FILE:
                    MessageBox.Show("RozWorld failed to load \\link\\guiometry.bin. Check it is present, and either reinstate a fresh copy of this file or make sure that you edited the file correctly." + detailsProvided + "\n\nRozWorld will now exit.",
                        "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                case Error.INVALID_GUI_DICTIONARY_KEY:
                    MessageBox.Show("A reference was made to a non-existent GUI control or control system. " + detailsProvided + "\n\nRozWorld will now exit.",
                        "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    break;

                case Error.MISSING_INI_DICTIONARY_KEY:
                    MessageBox.Show("A reference was made to a missing INI variable. " + detailsProvided + "\n\nRozWorld will now exit.",
                        "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error); 

                    break;

                default:
                case Error.UNKNOWN_ERROR:
                    MessageBox.Show("An unknown critical error occurred. " + detailsProvided + "\n\nRozWorld will now exit.",
                        "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    break;
            }

            Environment.Exit(1);
        }
    }
}
