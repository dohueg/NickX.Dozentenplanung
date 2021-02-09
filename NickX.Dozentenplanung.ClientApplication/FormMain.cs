using NickX.Dozentenplanung.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Transitions;

namespace NickX.Dozentenplanung.ClientApplication
{
    public partial class FormMain : Form
    {
        private bool main_menu_expanded;
        private Dictionary<Button, String> main_menu_buttons;
        private Dictionary<Button, String> view_change_buttons;
        private XCalendar _calendar;
        
        public static Session UserSession { get; set; }

        public FormMain()
        {
            UserSession = new Session()
            {
                ConnectionString = $""
            };


            InitializeComponent();
            this.SetStyle(ControlStyles.ResizeRedraw, true); // this is to avoid visual artifacts
            InitializeMainMenuButtons();
            InitializeViewChangeButtons();
            InitializeCalendar();
        }

        private void InitializeCalendar()
        {
            _calendar = new XCalendar();
            _calendar.Dock = DockStyle.Fill;
            _calendar.FillColorCurrentDateRow = Color.FromArgb(42, 191, 159);
            _calendar.StartDateTime = DateTime.Now.AddDays(1);
            _calendar.FirstDayOfWeek = DayOfWeek.Monday;
            _calendar.BackColor = Color.Transparent;
            _calendar.BorderColorGrid = calendar_holder.BackColor;
            _calendar.BorderColorGridColumn = Color.FromArgb(220, 220, 220);
            _calendar.BorderColorGridRow = Color.FromArgb(120, 120, 120);
            _calendar.FillColorWeekendDays = Color.White;
            _calendar.CalendarView = CalendarViews.Day;

            _calendar.Users = GetTestUsers();
            //using (var db = new XDbContext(UserSession.ConnectionString))
            //{
            //    var q = from u in db.CalendarUsers select u;
            //    _calendar.Users = q.ToList();
            //}

            calendar_holder.Controls.Add(_calendar);
        }

        private void InitializeMainMenuButtons()
        {
            main_menu_expanded = true;

            main_menu_buttons = new Dictionary<Button, string>();
            main_menu_buttons.Add(button_uebersicht, button_uebersicht.Text);
            main_menu_buttons.Add(button_meine_termine, button_meine_termine.Text);
            main_menu_buttons.Add(button_allgemein, button_allgemein.Text);
            main_menu_buttons.Add(button_benutzer, button_benutzer.Text);
            main_menu_buttons.Add(button_close_application, button_close_application.Text);

            foreach (var menu_button in main_menu_buttons.Keys)
            {
                menu_button.Click += MainMenuButton_Click;
            }
        }

        private void InitializeViewChangeButtons()
        {
            view_change_buttons = new Dictionary<Button, string>();
            view_change_buttons.Add(button_view_today, button_view_today.Text);
            view_change_buttons.Add(button_view_week, button_view_week.Text);
            view_change_buttons.Add(button_view_month, button_view_month.Text);

            foreach (var view_change_button in view_change_buttons.Keys)
            {
                view_change_button.Click += ViewChangeButton_Click;
            }
        }

        private void ViewChangeButton_Click(object sender, EventArgs e)
        {
            var clicked = (Button)sender;
            foreach (var btn in view_change_buttons.Keys)
            {
                btn.BackColor = Color.Gainsboro;
            }
            clicked.BackColor = Color.FromArgb(192, 241, 231);
        }

        private void MainMenuButton_Click(object sender, EventArgs e)
        {
            var clicked = (Button)sender;
            var txt = main_menu_buttons[clicked];
            label_current_page.Text = txt;

            foreach (var menu_button in main_menu_buttons.Keys)
            {
                if (menu_button != button_close_application)
                {
                    menu_button.BackColor = main_menu.BackColor;
                }
            }
            clicked.BackColor = Color.FromArgb(83, 88, 95);
        }

        private void button_collapse_main_menu_Click(object sender, EventArgs e)
        {
            var w = (main_menu_expanded) ? 50 : 200;
            var i = (main_menu_expanded) ? Properties.Resources.image_expand_17 : Properties.Resources.image_collapse_17;
            var p = (main_menu_expanded) ? 14 : 10;
            var h = (main_menu_expanded) ? 2 * 35 + 5 : 3 * 35 + 5;
            var t = new Transition(new EaseInEaseOutTransition(175));
            t.add(main_menu_holder, "Width", w);
            t.TransitionCompletedEvent += MainMenuCollapse_Complete;
            t.run();            
            button_collapse_main_menu.BackgroundImage = i;

            foreach (var menu_group in new List<Panel>() { menu_group_planung, menu_group_administration })
            {
                Transition.run(menu_group, "Height", h, new EaseInEaseOutTransition(175));
                //menu_group.Height = h;
            }
            foreach (var menu_button in main_menu_buttons.Keys)
            {
                var txt = (main_menu_expanded) ? "" : main_menu_buttons[menu_button];
                menu_button.Text = txt;
                menu_button.Padding = new Padding(menu_button.Padding.Left, menu_button.Padding.Top, p, menu_button.Padding.Bottom);
            }
            label_planung.Visible = !main_menu_expanded;
            label_administration.Visible = !main_menu_expanded;
            label_logo.Text = (main_menu_expanded) ? "" : "Dozentenplanung";

            main_menu_expanded = !main_menu_expanded;
        }

        private void MainMenuCollapse_Complete(object sender, Transition.Args e)
        {
            _calendar.Invalidate();
            _calendar.PopulateGrid();
        }

        #region Move Form by Dragging panel
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private void drag_panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion

        #region Resize Form 

        private void FormMain_Resize(object sender, EventArgs e)
        {
            //_calendar.Invalidate();
        }

        //protected override void OnPaint(PaintEventArgs e) // you can safely omit this method if you want
        //{
        //    e.Graphics.FillRectangle(Brushes.Green, Top);
        //    e.Graphics.FillRectangle(Brushes.Green, Left);
        //    e.Graphics.FillRectangle(Brushes.Green, Right);
        //    e.Graphics.FillRectangle(Brushes.Green, Bottom);
        //}

        private const int
            HTLEFT = 10,
            HTRIGHT = 11,
            HTTOP = 12,
            HTTOPLEFT = 13,
            HTTOPRIGHT = 14,
            HTBOTTOM = 15,
            HTBOTTOMLEFT = 16,
            HTBOTTOMRIGHT = 17;

        const int _ = 20; // you can rename this variable if you like

        new Rectangle Top { get { return new Rectangle(0, 0, this.ClientSize.Width, _); } }
        new Rectangle Left { get { return new Rectangle(0, 0, _, this.ClientSize.Height); } }
        new Rectangle Bottom { get { return new Rectangle(0, this.ClientSize.Height - _, this.ClientSize.Width, _); } }
        new Rectangle Right { get { return new Rectangle(this.ClientSize.Width - _, 0, _, this.ClientSize.Height); } }

        Rectangle TopLeft { get { return new Rectangle(0, 0, _, _); } }
        Rectangle TopRight { get { return new Rectangle(this.ClientSize.Width - _, 0, _, _); } }
        Rectangle BottomLeft { get { return new Rectangle(0, this.ClientSize.Height - _, _, _); } }
        Rectangle BottomRight { get { return new Rectangle(this.ClientSize.Width - _, this.ClientSize.Height - _, _, _); } }


        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            if (message.Msg == 0x0084) // WM_NCHITTEST
            {
                var cursor = this.PointToClient(Cursor.Position);

                if (TopLeft.Contains(cursor)) message.Result = (IntPtr)HTTOPLEFT;
                else if (TopRight.Contains(cursor)) message.Result = (IntPtr)HTTOPRIGHT;
                else if (BottomLeft.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMLEFT;
                else if (BottomRight.Contains(cursor)) message.Result = (IntPtr)HTBOTTOMRIGHT;

                else if (Top.Contains(cursor)) message.Result = (IntPtr)HTTOP;
                else if (Left.Contains(cursor)) message.Result = (IntPtr)HTLEFT;
                else if (Right.Contains(cursor)) message.Result = (IntPtr)HTRIGHT;
                else if (Bottom.Contains(cursor)) message.Result = (IntPtr)HTBOTTOM;
            }
        }
        #endregion
        private void FormMain_Paint(object sender, PaintEventArgs e)
        {
            //var p = this.Padding.All;
            //var rect_main_menu = new Rectangle(0, 0, main_menu_holder.Width + p, main_menu_holder.Height + 2 * p);
            //var rect_current_page = new Rectangle(main_menu_holder.Width + p, 0, label_current_page_holder.Width, 2);
            ////e.Graphics.FillRectangle(new SolidBrush(main_menu.BackColor), rect_main_menu);
            ////e.Graphics.FillRectangle(new SolidBrush(label_current_page.BackColor), rect_current_page);
        }

        private void button_close_application_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button_hide_details_panel_Click(object sender, EventArgs e)
        {
            var t = new Transition(new EaseInEaseOutTransition(175));
            t.add(panel_details, "ForeColor", panel_details.BackColor);
            t.TransitionCompletedEvent += T_TransitionCompletedEvent;
            t.run();
        }

        private void T_TransitionCompletedEvent(object sender, Transition.Args e)
        {
            var t = new Transition(new EaseInEaseOutTransition(175));
            t.add(panel_details, "Width", 0);
            t.run();
        }

        private void button_close_application_MouseEnter(object sender, EventArgs e)
        {
            ((Button)sender).ForeColor = Color.White;
        }

        private void button_close_application_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).ForeColor = Color.Silver;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            button_uebersicht.PerformClick();
            button_view_today.PerformClick();
        }

        private void button_view_today_Click(object sender, EventArgs e)
        {
            _calendar.CalendarView = CalendarViews.Day;
        }

        private void button_view_week_Click(object sender, EventArgs e)
        {
            _calendar.CalendarView = CalendarViews.Week;
        }

        private void button_view_month_Click(object sender, EventArgs e)
        {
            _calendar.CalendarView = CalendarViews.Month;
        }


        #region Data
        private List<XCalendarUser> GetTestUsers()
        {
            var u1 = new XCalendarUser()
            {
                Name = "User 1",
                Shortname = "dh",
                Color = Color.FromArgb(224, 138, 163)
            };
            var u2 = new XCalendarUser()
            {
                Name = "User 2",
                Shortname = "tb",
                Color = Color.FromArgb(195, 155, 194)
            };
            var u3 = new XCalendarUser()
            {
                Name = "User 3",
                Shortname = "dh",
                Color = Color.FromArgb(201, 184, 194)
            };
            var u4 = new XCalendarUser()
            {
                Name = "User 4",
                Shortname = "suw",
                Color = Color.FromArgb(156, 194, 208)
            };
            var u5 = new XCalendarUser()
            {
                Name = "User 5",
                Shortname = "bh",
                Color = Color.FromArgb(255, 194, 0)
            };
            var u6 = new XCalendarUser()
            {
                Name = "User 6",
                Shortname = "cv",
                Color = Color.FromArgb(119, 179, 75)
            };
            var u7 = new XCalendarUser()
            {
                Name = "User 7",
                Shortname = "mh",
                Color = Color.FromArgb(173, 134, 94)
            };

            return new List<XCalendarUser>()
            {
                u1,
                u2,
                u3,
                u4,
                u5,
                u6,
                u7
            };
        }
        #endregion
    }
}
