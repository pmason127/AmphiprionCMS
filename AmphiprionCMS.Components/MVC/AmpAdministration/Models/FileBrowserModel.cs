using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using System.Linq;
using System.Web;

namespace AmphiprionCMS.Models
{
    public class FileBrowserModel
    {
        public FileBrowserModel()
        {
            Files = new Collection<File>();
            Folders = new Collection<Folder>();
        }
        public string CurrentPath { get; set; }
        public ICollection<Folder> Folders { get; set; }
        public ICollection<File> Files { get; set; }
        public string Filter { get; set; }
        public bool HasData
        {
            get
            {
                return Folders != null && Files != null && (Folders.Any() || Files.Any());
            }
        }
    }

    public class Folder
    {
        public string Name { get; set; }
        public bool HasFiles { get; set; }
    }
    public class File
    {
        public string Name { get; set; }
        public bool Extension { get; set; }
        public bool IsImage { get; set; }
    }

}