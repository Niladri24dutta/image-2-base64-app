using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ImageConverter
{
    public partial class ConverterForm : Form
    {
        SaveFileDialog save = new SaveFileDialog();
        public ConverterForm()
        {
            InitializeComponent();
        }

        

        private void Form1_Load(object sender, EventArgs e)
        {
            deleteFiles();
        }

        

        private void deleteFiles()
        {
            string[] filenames = null;
            filenames = Directory.GetFiles(Path.GetDirectoryName(getTempfilepath()));
            foreach (string filename in filenames)
            {
                File.Delete(filename);
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            string inputfilepath = string.Empty;
            textBox1.Text = "";
            textBox2.Text = "";
            openFileDialog1.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF|" + "All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.ShowHelp = true;
            openFileDialog1.FileName = "image.jpg";
            DialogResult result = openFileDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                inputfilepath = openFileDialog1.FileName;
                File.Copy(inputfilepath,getTempfilepath(), true);
                showThumbnail(getTempfilepath());
                label1.Visible = true;
                pictureBox2.Visible = true;
                button2.Visible = true;
                label4.Visible = true;
                panel2.Visible = true;

                
            }
        }

        private string getTempfilepath()
        {
            
            string currentdir, currentdirpath, fileextension = string.Empty;
            string temppath = string.Empty;
            
            fileextension = Path.GetExtension(openFileDialog1.SafeFileName);
            
            currentdirpath = AppDomain.CurrentDomain.BaseDirectory;
            currentdir = Path.GetDirectoryName(currentdirpath);
            if (currentdir.Contains("\\bin\\Debug"))
            {
                temppath = currentdir.Replace("\\bin\\Debug", "\\Tempimages\\");
            }

            if (currentdir.Contains("\\bin\\Release"))
            {
                temppath = currentdir.Replace("\\bin\\Release", "\\Tempimages\\");
            }

            temppath = temppath + "tempimage" + fileextension;

            return temppath;
        }

        private void showThumbnail(string filepath)
        {
            pictureBox2.Height = 100;
            pictureBox2.Width = 100;
            try
            {
                Image.GetThumbnailImageAbort callbackfunction = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                Bitmap bmp = new Bitmap(filepath);
                Image thumbnail = bmp.GetThumbnailImage(96, 96, callbackfunction, IntPtr.Zero);
                pictureBox2.Image = thumbnail;
                 
            }
            catch(Exception err)
            {
                MessageBox.Show("Error :" + err.Message);
            }
         }

        
      public bool ThumbnailCallback()
        {
            return false;
        }

      private void radioButton3_CheckedChanged(object sender, EventArgs e)
      {
          if(radioButton3.Checked)
          {
              label5.Visible = true;
              alttext.Visible = true;
              
              textBox1.Text = "";
              label6.Visible = false;
              textBox1.Visible = false;
              button3.Visible = false;
              button4.Visible = false;
             
              label7.Visible = false;

          }
      }

      private void radioButton4_CheckedChanged(object sender, EventArgs e)
      {
          if (radioButton4.Checked)
          {
              label5.Visible =  false;
              alttext.Visible = false;
              alttext.Text = "";
             
              textBox1.Text = "";
              label6.Visible = false;
              textBox1.Visible = false;
              label7.Visible = false;
              textBox2.Visible = false;
             
              textBox2.Text = "";
              button3.Visible = false;
              button4.Visible = false;
          }
      }

      private void button2_Click(object sender, EventArgs e)
      {
          string base64string = null;
          
          string extension,alternatetext,htmlstring = string.Empty;
          
          if(!string.IsNullOrEmpty(openFileDialog1.SafeFileName))
          {
              using (System.Drawing.Image img = System.Drawing.Image.FromFile(getTempfilepath()))
              {
                  using (MemoryStream mo = new MemoryStream())
                  {
                      img.Save(mo, img.RawFormat);
                      byte[] imgbytes = mo.ToArray();
                      base64string = Convert.ToBase64String(imgbytes);
                      textBox1.Text = base64string;
                  }
                  
              }
              label6.Visible = true;
              button3.Visible = true;
              textBox1.Visible = true;
              if (radioButton3.Checked)
              {
                 if (!string.IsNullOrEmpty(alttext.Text.ToString()))
                  {
                      alternatetext = alttext.Text.ToString();
                  }
                  else
                  {
                      alternatetext = Path.GetFileNameWithoutExtension(openFileDialog1.SafeFileName);
                  }

                  extension = Path.GetExtension(openFileDialog1.SafeFileName);
                  extension = extension.Trim('.');
                  htmlstring = "<img src=\"data:image/" + extension + ";base64," + base64string + "\"" + " alt=\"" + alternatetext + "\" >";
                  textBox2.Text = htmlstring;
                  
                  label7.Visible = true;
                  button4.Visible = true;
                  textBox2.Visible = true;
                  
              }
              
          }
          else
          {
              label2.Text = "Please select a file to upload";
              return;
          }
      }

       private void copyToClipboard(string text)
      {
          Clipboard.SetText(text);
          MessageBox.Show("text copied", "Success!");
      }

       private void button3_Click(object sender, EventArgs e)
       {
           copyToClipboard(textBox1.Text);
       }

       private void button4_Click(object sender, EventArgs e)
       {
           copyToClipboard(textBox2.Text);
       }

      }
}
