﻿using System;

namespace Net.Sf.Dbdeploy
{
    using System.Globalization;

    /// <summary>
    /// Represents any unique change.
    /// </summary>
    public abstract class UniqueChange : IComparable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UniqueChange" /> class.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="scriptNumber">The script number.</param>
        protected UniqueChange(string folder, int scriptNumber)
        {
            this.Folder = folder;
            this.ScriptNumber = scriptNumber;

            Version version;
            Version.TryParse(folder.TrimStart('v'), out version);
            this.Version = version;
        }

        /// <summary>
        /// Gets the folder that contained the script executed.
        /// </summary>
        /// <value>
        /// The folder.
        /// </value>
        public string Folder { get; private set; }

        /// <summary>
        /// Gets the script number executed inside the folder.
        /// </summary>
        /// <value>
        /// The script number.
        /// </value>
        public int ScriptNumber { get; private set; }

        /// <summary>
        /// Gets the version of the folder if in the format of (v1.0.0.0) or (1.0.0).
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public Version Version { get; private set; }

        /// <summary>
        /// Gets the unique key that represents this change on the file system.
        /// </summary>
        /// <value>
        /// The unique key.
        /// </value>
        public string UniqueKey
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture, "{0}/{1}", this.Folder, this.ScriptNumber);
            }
        }

        /// <summary>
        /// Compares the object to another giving a result of less than, equal to, or greater than.
        /// </summary>
        /// <param name="obj">The object to compare.</param>
        /// <returns>Integer value corresponding to less than, equal to, or greater than.</returns>
        public int CompareTo(object obj)
        {
            var other = (UniqueChange)obj;

            // Compare versions if both folders are in version format v1.0.0.0.
            int result;
            if (this.Version != null && other.Version != null)
            {
                result = this.Version.CompareTo(other.Version);
            }
            else
            {
                // Compare folder names as is.
                result = string.Compare(this.Folder, other.Folder, StringComparison.InvariantCultureIgnoreCase);
            }

            // Compare script number of folders are the same.
            if (result == 0)
            {
                result = this.ScriptNumber.CompareTo(other.ScriptNumber);
            }

            return result;
        }
    }
}