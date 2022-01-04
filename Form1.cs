using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace MinecraftUUID
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new();
            dialog.Description = "选择目录"; //提示文字
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string strSaveFolder = dialog.SelectedPath;
                textBox1.Text = strSaveFolder;
                DeleteDir(@textBox1.Text + @"\.idea");
            }

        }

        // 生成 | 替换
        private void button2_Click(object sender, EventArgs e)
        {
            // 清空文本框
            textBox2.Text = "";
            DirectoryInfo TheFolder = new DirectoryInfo(textBox1.Text);
            //遍历文件夹
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            {

                string fileName = textBox1.Text + @"\" + NextFolder.Name + @"\manifest.json";
                textBox2.Text += fileName + "\r\n";
                JObject json = ReadFileJson(@fileName);
                json["header"]["uuid"] = Guid.NewGuid().ToString();
                json["modules"][0]["uuid"] = Guid.NewGuid().ToString();
                WriteJson(@fileName, json);

            }

            MessageBox.Show("替换成功!");


        }


        /// <summary>
        /// 读取JSON文件
        /// </summary>
        private JObject? ReadFileJson(string jsonFile)
        {
            try
            {
                using StreamReader file = System.IO.File.OpenText(jsonFile);
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject jObject = (JObject)JToken.ReadFrom(reader);
                    return jObject;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// 写入JSON文件
        /// </summary>
        /// <param name="jsonFile"></param>
        /// <param name="jObject"></param>
        private void WriteJson(string jsonFile, JObject jObject)
        {
            using StreamWriter file = new StreamWriter(jsonFile);
            file.Write(jObject.ToString());
        }

        /// <summary>
        /// 直接删除指定目录下的所有文件及文件夹(保留目录)
        /// </summary>
        /// <param name="strPath">文件夹路径</param>
        /// <returns>执行结果</returns>
        public bool DeleteDir(string strPath)
        {
            try
            {
                // 清除空格
                strPath = @strPath.Trim().ToString();
                // 判断文件夹是否存在
                if (System.IO.Directory.Exists(strPath))
                {
                    // 获得文件夹数组
                    string[] strDirs = System.IO.Directory.GetDirectories(strPath);
                    // 获得文件数组
                    string[] strFiles = System.IO.Directory.GetFiles(strPath);
                    // 遍历所有子文件夹
                    foreach (string strFile in strFiles)
                    {
                        // 删除文件夹
                        System.IO.File.Delete(strFile);
                    }
                    // 遍历所有文件
                    foreach (string strdir in strDirs)
                    {
                        // 删除文件
                        System.IO.Directory.Delete(strdir, true);
                    }
                    // 删除目录
                    Directory.Delete(strPath, true);
                }
                // 成功
                return true;
            }
            catch (Exception Exp) // 异常处理
            {
                // 异常信息
                System.Diagnostics.Debug.Write(Exp.Message.ToString());
                // 失败
                return false;
            }
        }
    }
}