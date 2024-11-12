namespace FileSystem.Utils
{
    public class ProductUtils
    {
        public static void CopyFolder(DirectoryInfo source, DirectoryInfo target)
        {
            try
            {
                if (!Directory.Exists(target.FullName))
                    Directory.CreateDirectory(target.FullName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());

                Thread.Sleep(1000);
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                try
                {
                    fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());

                    Thread.Sleep(1000);
                    fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
                }
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyFolder(diSourceSubDir, nextTargetSubDir);
            }
        }

        public static void DeleteFiles(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    var dir = new DirectoryInfo(path);
                    dir.Attributes = dir.Attributes & ~FileAttributes.ReadOnly;
                    dir.Delete(true);
                }
            }
            catch (Exception ex) { }
        }

        public static bool FileExist(string folder, string ProductId, string Image)
        {
            string imageName = folder + "\\" + ProductId + "\\" + "1" + "\\" + Image;
            return File.Exists(imageName);
        }

        public static bool FileExist(string folder, string ProductId, string index, string Image)
        {
            string imageName = folder + "\\" + ProductId + "\\" + index + "\\" + Image;
            return File.Exists(imageName);
        }

        public static string GetProductId(long phoneno, int pincode)
        {
            string productId = new string("");
            
            productId += phoneno.ToString()[0];
            productId += phoneno.ToString()[1];
            productId += pincode.ToString()[4];
            productId += pincode.ToString()[5];
            productId += phoneno.ToString()[8];
            productId += phoneno.ToString()[9];

            return productId;
        }

        public static string GetMonth(int month)
        {
            switch(month)
            {
                case 1:
                    return "January";
                case 2:
                    return "February";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
                default:
                    return "January";
            }

            return "January";
        }
    }
}
