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
            dialog.Description = "ѡ��Ŀ¼"; //��ʾ����
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string strSaveFolder = dialog.SelectedPath;
                textBox1.Text = strSaveFolder;
                DeleteDir(@textBox1.Text + @"\.idea");
            }

        }

        // ���� | �滻
        private void button2_Click(object sender, EventArgs e)
        {
            // ����ı���
            textBox2.Text = "";
            DirectoryInfo TheFolder = new DirectoryInfo(textBox1.Text);
            //�����ļ���
            foreach (DirectoryInfo NextFolder in TheFolder.GetDirectories())
            {

                string fileName = textBox1.Text + @"\" + NextFolder.Name + @"\manifest.json";
                textBox2.Text += fileName + "\r\n";
                JObject json = ReadFileJson(@fileName);
                json["header"]["uuid"] = Guid.NewGuid().ToString();
                json["modules"][0]["uuid"] = Guid.NewGuid().ToString();
                WriteJson(@fileName, json);

            }

            MessageBox.Show("�滻�ɹ�!");


        }


        /// <summary>
        /// ��ȡJSON�ļ�
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
        /// д��JSON�ļ�
        /// </summary>
        /// <param name="jsonFile"></param>
        /// <param name="jObject"></param>
        private void WriteJson(string jsonFile, JObject jObject)
        {
            using StreamWriter file = new StreamWriter(jsonFile);
            file.Write(jObject.ToString());
        }

        /// <summary>
        /// ֱ��ɾ��ָ��Ŀ¼�µ������ļ����ļ���(����Ŀ¼)
        /// </summary>
        /// <param name="strPath">�ļ���·��</param>
        /// <returns>ִ�н��</returns>
        public bool DeleteDir(string strPath)
        {
            try
            {
                // ����ո�
                strPath = @strPath.Trim().ToString();
                // �ж��ļ����Ƿ����
                if (System.IO.Directory.Exists(strPath))
                {
                    // ����ļ�������
                    string[] strDirs = System.IO.Directory.GetDirectories(strPath);
                    // ����ļ�����
                    string[] strFiles = System.IO.Directory.GetFiles(strPath);
                    // �����������ļ���
                    foreach (string strFile in strFiles)
                    {
                        // ɾ���ļ���
                        System.IO.File.Delete(strFile);
                    }
                    // ���������ļ�
                    foreach (string strdir in strDirs)
                    {
                        // ɾ���ļ�
                        System.IO.Directory.Delete(strdir, true);
                    }
                    // ɾ��Ŀ¼
                    Directory.Delete(strPath, true);
                }
                // �ɹ�
                return true;
            }
            catch (Exception Exp) // �쳣����
            {
                // �쳣��Ϣ
                System.Diagnostics.Debug.Write(Exp.Message.ToString());
                // ʧ��
                return false;
            }
        }
    }
}