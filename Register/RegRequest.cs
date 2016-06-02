using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Register
{
    public partial class RegRequest : Form
    {
        public RegRequest()
        {
            InitializeComponent();
        }

        public string SoftWareName = "";

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == "")
            {
                MessageBox.Show("软件/网站名称 不能为空，获取软件/网站名称失败！");
                return;
            }
            if (textBox1.Text.Trim() == "")
            {
                MessageBox.Show("用户姓名/单位名 不能为空！");
                return;
            }
            if (textBox3.Text.Trim() == "")
            {
                MessageBox.Show("接收注册码Email 不能为空！");
                return;
            }
            //判断接收注册码Email的合法性
            //Regex reg = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            if (!Regex.IsMatch(textBox3.Text.Trim(), @"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"))
            {
                MessageBox.Show("接收注册码Email不合法，请重新输入！");
                return;
            }
            ComputerInfo.Computer com = ComputerInfo.Computer.Instance();
            string HardDiskSN = com.DiskPhysicalSN;
            string userName = textBox1.Text.Trim();
            string softWareName = textBox2.Text.Trim();
            string receiveEmail = textBox3.Text.Trim();
            //发送邮件
            try
            {
                string from = "wonderful-abc@163.com";//发送人必须为163邮件地址
                string to = "wonderful-abc@163.com";//邮件接收地址
                string title = "软件客户注册";
                string mailContent = "\n来自 " + userName + " 的软件 " + softWareName + " 需要注册，其注册申请码为 [" + HardDiskSN + "] 。\n请及时回复其注册码，回复地址为：" + receiveEmail;
                MailMessage mm = new MailMessage(from, to, title, mailContent);
                mm.SubjectEncoding = System.Text.Encoding.UTF8;
                mm.BodyEncoding = System.Text.Encoding.UTF8;
                
                //"为什么Smtp类的Server属性不提供口令验证";
                //mm.Attachments.Add(new MailAttachment("c:\\test.txt"));
                //Attachments实现IList接口，可以添加多个附件
                SmtpClient sc = new SmtpClient("smtp.163.com", 25);//pop.163.com
                sc.DeliveryMethod = SmtpDeliveryMethod.Network;
                //sc.UseDefaultCredentials = true;
                //sc.Credentials = CredentialCache.DefaultNetworkCredentials;
                sc.Credentials = new System.Net.NetworkCredential("wonderful-abc", "11805129837159");//不可或缺，必须为发送人邮箱的登录帐号和密码
                sc.Send(mm);
                //SmtpMail.SmtpServer = "webmail.careland.com.cn";
                //设置发送邮件的Smtp服务器，目前没有提供需要身份验证的服务器的登陆方法
                //比如163.net,163.com都不行，目前我的是我公司的SMTP服务器
                //看看大伙有没有更好的办法
                //SmtpMail.Send(mm);
                mm.Dispose();
                MessageBox.Show("已经成功提交注册信息！\n请您留意在电子邮箱 " + receiveEmail + " 里查收注册码！\n谢谢您的支持！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (SoftWareName != "")
            {
                textBox2.Text = SoftWareName;
            }
            else
            {
                MessageBox.Show("自动获取软件名称失败，请手动填写！");
                checkBox1.Checked = true;
                textBox2.Enabled = true;
                textBox2.Focus();
                textBox2.SelectAll();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.Enabled = true;
                textBox2.Focus();
                textBox2.SelectAll();
            }
            else
            {
                textBox2.Enabled = false;
            }
        }
    }
}