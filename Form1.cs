using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace file_manager
{
    public partial class GoBack2 : Form
    {

        public GoBack2()
        {
            InitializeComponent();
            InitializeContextMenu();
            
        }

        public void InitializeContextMenu()
        {
            
            ContextMenuStrip contextMenu = new ContextMenuStrip();
           
            ToolStripMenuItem copyItem = new ToolStripMenuItem("Копировать", null, CopyTo_Click);
            ToolStripMenuItem pasteItem = new ToolStripMenuItem("Вставить", null, MoveTo_Click);
            ToolStripMenuItem deleteItem = new ToolStripMenuItem("Удалить", null, Delete_Click);
            ToolStripMenuItem renameItem = new ToolStripMenuItem("Переименовать", null, RenameIt_Click);
            ToolStripMenuItem archiveItem = new ToolStripMenuItem("Архивировать", null, GZipStream_Click);

            contextMenu.Items.AddRange(new ToolStripItem[] { copyItem, pasteItem, deleteItem, renameItem, archiveItem });

            listBox1.ContextMenuStrip = contextMenu;
        }
        public void GoOver_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();

            DirectoryInfo Dir = new DirectoryInfo(textBox1.Text);
            DirectoryInfo[] Dirs = Dir.GetDirectories();
            foreach (DirectoryInfo CurrentDir in Dirs)
            {
                listBox1.Items.Add(CurrentDir.FullName);

            }

            FileInfo[] files = Dir.GetFiles();

            foreach (FileInfo CurrentFile in files)
            {
                listBox1.Items.Add(CurrentFile.FullName);
            }

        }

        public void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (Path.GetExtension(Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString())) == "")
            {
                textBox1.Text = Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString());
                listBox1.Items.Clear();

                DirectoryInfo Dir = new DirectoryInfo(textBox1.Text);
                DirectoryInfo[] Dirs = Dir.GetDirectories();
                foreach (DirectoryInfo CurrentDir in Dirs)
                {
                    listBox1.Items.Add(CurrentDir.FullName);

                }

                FileInfo[] files = Dir.GetFiles();

                foreach (FileInfo CurrentFile in files)
                {
                    listBox1.Items.Add(CurrentFile.FullName);
                }
            }
            else
            {
                Process.Start(Path.Combine(textBox1.Text, listBox1.SelectedItem.ToString()));
            }


        }

        public void GoBack_Click(object sender, EventArgs e)
        {
            if (textBox1.Text[textBox1.Text.Length - 1] == '\\')
            {
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                while (textBox1.Text[textBox1.Text.Length - 1] != '\\')
                {
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                }
            }

            else if (textBox1.Text[textBox1.Text.Length - 1] != '\\')
            {
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                while (textBox1.Text[textBox1.Text.Length - 1] != '\\')
                {
                    textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1, 1);
                }
            }

            listBox1.Items.Clear();

            DirectoryInfo Dir = new DirectoryInfo(textBox1.Text);
            DirectoryInfo[] Dirs = Dir.GetDirectories();
            foreach (DirectoryInfo CurrentDir in Dirs)
            {
                listBox1.Items.Add(CurrentDir.FullName);

            }

            FileInfo[] files = Dir.GetFiles();

            foreach (FileInfo CurrentFile in files)
            {
                listBox1.Items.Add(CurrentFile.FullName);
            }
        }
        

        public void listBox1_Click(object sender, EventArgs e)
        {
            listBox1.Focus();
        }



        public void Delete_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }

        public void CopyTo_Click(object sender, EventArgs e)
        {
           CopyItem();
        }


        public static class Prompt
        {
            public static string ShowDialog(string text, string caption)
            {
                Form prompt = new Form()
                {
                    Width = 400,
                    Height = 200,
                    Text = caption
                };

                Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
                System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox() { Left = 50, Top = 50, Width = 300 };
                System.Windows.Forms.Button myButton = new System.Windows.Forms.Button() { Text = "ОК", Left = 250, Width = 100, Top = 100 };
                myButton.Click += (sender, e) => { prompt.Close(); };

                prompt.Controls.Add(textLabel);
                prompt.Controls.Add(textBox);
                prompt.Controls.Add(myButton);

                prompt.ShowDialog();
                return textBox.Text;
            }
        }


        public void MoveTo_Click(object sender, EventArgs e)
        {
            PasteItem();
            
        }

        public void RenameIt_Click(object sender, EventArgs e)
        {
           RenameItem();
        }

        public void GZipStream_Click(object sender, EventArgs e)
        {
            ArchiveItem();
        }
    

    public void Form1_Load(object sender, EventArgs e)
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady) 
                {
                    comboBox1.Items.Add(drive.Name);
                }
            }
        }
        public void LoadDirectoriesAndFiles()
        {
            listBox1.Items.Clear(); 

            DirectoryInfo Dir = new DirectoryInfo(textBox1.Text);

            DirectoryInfo[] Dirs = Dir.GetDirectories();
            foreach (DirectoryInfo CurrentDir in Dirs)
            {
                listBox1.Items.Add(CurrentDir.FullName);
            }

            FileInfo[] files = Dir.GetFiles();
            foreach (FileInfo CurrentFile in files)
            {
                listBox1.Items.Add(CurrentFile.FullName);
            }
        }

        public void GoBack2_Load(object sender, EventArgs e)
        {
            foreach (var drive in DriveInfo.GetDrives())
            {
                if (drive.IsReady) 
                {
                    comboBox1.Items.Add(drive.Name);
                }
            }
        }

        public void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = comboBox1.SelectedItem.ToString();
            LoadDirectoriesAndFiles();
        }

        public string copiedPath;
        public void CopyItem()
        {
            if (listBox1.SelectedItem != null)
            {
                copiedPath = listBox1.SelectedItem.ToString();
                MessageBox.Show("Элемент скопирован: " + copiedPath);
            }
        }
        public void PasteItem()
        {
            if (!string.IsNullOrEmpty(copiedPath))
            {
                string NextPath = textBox1.Text;
                string itemName = Path.GetFileName(copiedPath);
                string FullPath = Path.Combine(NextPath, itemName);

                if (Directory.Exists(copiedPath))
                {
                    DirectoryCopy(copiedPath, FullPath, true);
                }
                else
                {
                    File.Copy(copiedPath, FullPath, true);
                }

                MessageBox.Show("Элемент вставлен: " + FullPath);
                GoOver_Click(null, null);
            }
        }

        public void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("Источник не существует: " + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, false);
            }

            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                }
            }
        }




        public void HandleException(Exception ex)
        {
            if (ex is UnauthorizedAccessException)
            {
                MessageBox.Show("Ошибка: У вас нет прав для выполнения этой операции.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (ex is IOException)
            {
                MessageBox.Show("Ошибка: Произошла ошибка ввода-вывода. " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Неизвестная ошибка: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void DeleteItem()
        {
            if (listBox1.SelectedItem != null)
            {
                string pathToDelete = listBox1.SelectedItem.ToString();
                if (MessageBox.Show("Вы уверены, что хотите удалить " + pathToDelete + "?", "Подтверждение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        if (Directory.Exists(pathToDelete))
                        {
                            Directory.Delete(pathToDelete, true);
                        }
                        else
                        {
                            File.Delete(pathToDelete);
                        }
                        MessageBox.Show("Элемент удален: " + pathToDelete);
                        GoOver_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }
                }
            }
        }

        public void RenameItem()
        {
            if (listBox1.SelectedItem != null)
            {
                string oldPath = listBox1.SelectedItem.ToString();
                string newName = Prompt.ShowDialog("Введите новое имя:", "Переименовать");

                if (!string.IsNullOrEmpty(newName))
                {
                    string newPath = Path.Combine(Path.GetDirectoryName(oldPath), newName);
                    try
                    {
                        if (Directory.Exists(oldPath))
                        {
                            Directory.Move(oldPath, newPath);
                        }
                        else
                        {
                            File.Move(oldPath, newPath);
                        }
                        MessageBox.Show("Элемент переименован в: " + newPath);
                        GoOver_Click(null, null);
                    }
                    catch (Exception ex)
                    {
                        HandleException(ex);
                    }
                }
            }
        }

        public void ArchiveItem()
        {
            if (listBox1.SelectedItem != null)
            {
                string pathToArchive = listBox1.SelectedItem.ToString();
                string archivePath = pathToArchive + ".zip";

                try
                {
                    if (Directory.Exists(pathToArchive))
                    {
                        ZipFile.CreateFromDirectory(pathToArchive, archivePath);
                    }
                    else if (File.Exists(pathToArchive))
                    {
                        using (FileStream fs = new FileStream(archivePath, FileMode.Create))
                        using (ZipArchive archive = new ZipArchive(fs, ZipArchiveMode.Create))
                        {
                            archive.CreateEntryFromFile(pathToArchive, Path.GetFileName(pathToArchive));
                        }
                    }
                    else
                    {
                        MessageBox.Show("Указанный путь не существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MessageBox.Show("Элемент заархивирован: " + archivePath);
                }
                catch (Exception ex)
                {
                    HandleException(ex);
                }
            }
        }
    }
}
//ошибки и комбобокс
 