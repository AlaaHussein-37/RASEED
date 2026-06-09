using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Raseed;

public partial class MainWindow : Window
{
    private readonly Brush Primary = Brush("#2563EB");
    private readonly Brush PrimaryHover = Brush("#1D4ED8");
    private readonly Brush Success = Brush("#16A34A");
    private readonly Brush Warning = Brush("#F59E0B");
    private readonly Brush Danger = Brush("#DC2626");
    private readonly Brush BackgroundBrush = Brush("#F8FAFC");
    private readonly Brush CardBrush = Brush("#FFFFFF");
    private readonly Brush BorderBrushSoft = Brush("#E2E8F0");
    private readonly Brush TextPrimary = Brush("#0F172A");
    private readonly Brush TextSecondary = Brush("#64748B");
    private Border _pageHost = null!;
    private TextBlock _pageTitle = null!;
    private readonly List<Button> _navButtons = [];

    public MainWindow()
    {
        InitializeComponent();
        ShowLogin();
    }

    private static SolidColorBrush Brush(string hex) => (SolidColorBrush)new BrushConverter().ConvertFromString(hex)!;

    private void ShowLogin()
    {
        RootHost.Children.Clear();
        RootHost.Background = BackgroundBrush;

        var shell = new Grid { Margin = new Thickness(24), FlowDirection = FlowDirection.LeftToRight };
        shell.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1.1, GridUnitType.Star) });
        shell.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(0.9, GridUnitType.Star) });

        var brand = new StackPanel { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, FlowDirection = FlowDirection.RightToLeft };
        brand.Children.Add(new Image { Source = Logo(), Width = 250, Height = 180, Stretch = Stretch.Uniform });
        brand.Children.Add(Text("رصيد", 48, FontWeights.Bold, Primary, HorizontalAlignment.Center));
        brand.Children.Add(Text("إدارة الإجازات", 24, FontWeights.Medium, TextPrimary, HorizontalAlignment.Center));
        shell.Children.Add(brand);

        var panel = Card(new Thickness(0), 16);
        panel.Width = 420;
        panel.HorizontalAlignment = HorizontalAlignment.Center;
        panel.VerticalAlignment = VerticalAlignment.Center;
        Grid.SetColumn(panel, 1);
        var form = new StackPanel { Margin = new Thickness(32), FlowDirection = FlowDirection.RightToLeft };
        form.Children.Add(Text("مرحباً بك في رصيد", 22, FontWeights.Bold, TextPrimary));
        form.Children.Add(Text("سجل الدخول للمتابعة", 13, FontWeights.Medium, TextSecondary, margin: new Thickness(0, 4, 0, 24)));
        form.Children.Add(Label("اسم المستخدم"));
        form.Children.Add(Input("أدخل اسم المستخدم"));
        form.Children.Add(Label("كلمة المرور", new Thickness(0, 16, 0, 6)));
        form.Children.Add(Input("أدخل كلمة المرور"));
        form.Children.Add(Check("تذكرني", new Thickness(0, 16, 0, 18)));
        var login = Button("تسجيل الدخول", Primary, Brushes.White, 44);
        login.Click += (_, _) => ShowShell();
        form.Children.Add(login);
        panel.Child = form;
        shell.Children.Add(panel);

        RootHost.Children.Add(shell);
    }

    private void ShowShell()
    {
        RootHost.Children.Clear();
        var root = new Grid { Background = BackgroundBrush, FlowDirection = FlowDirection.LeftToRight };
        root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(240) });
        root.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(60) });
        root.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

        var top = new Border { Background = CardBrush, BorderBrush = BorderBrushSoft, BorderThickness = new Thickness(0, 0, 0, 1) };
        Grid.SetColumnSpan(top, 2);
        var topGrid = new Grid { Margin = new Thickness(20, 0, 20, 0), FlowDirection = FlowDirection.RightToLeft };
        topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        topGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        _pageTitle = Text("الرئيسية", 20, FontWeights.Bold, TextPrimary, VerticalAlignment.Center);
        topGrid.Children.Add(_pageTitle);
        var user = new StackPanel { Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Center, FlowDirection = FlowDirection.RightToLeft };
        user.Children.Add(Text("أحمد محمد", 13, FontWeights.Bold, TextPrimary, margin: new Thickness(0, 0, 0, 0)));
        user.Children.Add(Text(DateTime.Now.ToString("yyyy/MM/dd"), 13, FontWeights.Medium, TextSecondary, margin: new Thickness(0, 0, 14, 0)));
        Grid.SetColumn(user, 1);
        topGrid.Children.Add(user);
        top.Child = topGrid;
        root.Children.Add(top);

        root.Children.Add(Sidebar());
        _pageHost = new Border { Margin = new Thickness(18), Background = Brushes.Transparent };
        Grid.SetColumn(_pageHost, 1);
        Grid.SetRow(_pageHost, 1);
        root.Children.Add(_pageHost);
        RootHost.Children.Add(root);
        Navigate("الرئيسية");
    }

    private UIElement Sidebar()
    {
        var side = new Border { Background = Brush("#082F49"), FlowDirection = FlowDirection.RightToLeft };
        Grid.SetRow(side, 1);
        var wrap = new DockPanel { LastChildFill = true };
        var logo = new StackPanel { Margin = new Thickness(18, 18, 18, 16), Orientation = Orientation.Horizontal };
        logo.Children.Add(new Image { Source = Logo(), Width = 44, Height = 44, Stretch = Stretch.Uniform });
        logo.Children.Add(Text("رصيد", 22, FontWeights.Bold, Brushes.White, VerticalAlignment.Center, margin: new Thickness(10, 0, 0, 0)));
        DockPanel.SetDock(logo, Dock.Top);
        wrap.Children.Add(logo);

        var nav = new StackPanel { Margin = new Thickness(12, 4, 12, 12) };
        foreach (var item in new[] {
            ("🏠", "الرئيسية"),
            ("👥", "الموظفون"),
            ("📅", "الإجازات الاعتيادية"),
            ("🏥", "الإجازات المرضية"),
            ("⏱", "الإجازات الزمنية"),
            ("✅", "الموافقات"),
            ("📊", "التقارير"),
            ("⚙", "الإعدادات"),
            ("🚪", "تسجيل الخروج") })
        {
            var title = item.Item2;
            var button = Button(string.Empty, Brushes.Transparent, Brushes.White, 40);
            button.Content = NavContent(item.Item1, item.Item2);
            button.FlowDirection = FlowDirection.RightToLeft;
            button.HorizontalContentAlignment = HorizontalAlignment.Right;
            button.Margin = new Thickness(0, 4, 0, 0);
            button.Tag = title;
            button.Click += (_, _) => { if (title == "تسجيل الخروج") ShowLogin(); else Navigate(title); };
            _navButtons.Add(button);
            nav.Children.Add(button);
        }
        wrap.Children.Add(nav);
        side.Child = wrap;
        return side;
    }

    private void Navigate(string page)
    {
        _pageTitle.Text = page;
        foreach (var button in _navButtons)
            button.Background = Equals(button.Tag, page) ? Primary : Brushes.Transparent;

        _pageHost.Child = page switch
        {
            "الرئيسية" => Dashboard(),
            "الموظفون" => Employees(),
            "الإجازات الاعتيادية" => RegularLeave(),
            "الإجازات المرضية" => SickLeave(),
            "الإجازات الزمنية" => HourlyLeave(),
            "الموافقات" => Approvals(),
            "التقارير" => Reports(),
            "الإعدادات" => Settings(),
            _ => Dashboard()
        };
    }

    private UIElement Dashboard()
    {
        var page = Page();
        var cards = new UniformGrid { Columns = 5, Margin = new Thickness(0, 0, 0, 18) };
        foreach (var c in new[] {
            ("👥", "128", "عدد الموظفين", Primary),
            ("📅", "23", "الإجازات الحالية", Warning),
            ("✅", "14", "الإجازات المعلقة", Danger),
            ("🏥", "7", "الإجازات المرضية", Primary),
            ("⏱", "12", "الإجازات الزمنية", Success) })
            cards.Children.Add(StatCard(c.Item1, c.Item2, c.Item3, c.Item4));
        page.Children.Add(cards);
        page.Children.Add(Section("أحدث الطلبات", RequestGrid()));
        return Scroll(page);
    }

    private UIElement Employees()
    {
        var page = Page();
        var filters = new Grid { Margin = new Thickness(0, 0, 0, 16) };
        filters.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
        filters.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        filters.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        filters.Children.Add(Input("بحث بالاسم أو الرقم الوظيفي"));
        var combo = Combo("كل الأقسام", "تقنية المعلومات", "الموارد البشرية", "المالية", "التسويق");
        Grid.SetColumn(combo, 1);
        filters.Children.Add(combo);
        var add = Button("+ إضافة موظف", Primary, Brushes.White, 42);
        add.Click += (_, _) => _pageHost.Child = AddEmployee();
        Grid.SetColumn(add, 2);
        filters.Children.Add(add);
        page.Children.Add(filters);
        page.Children.Add(Section("قائمة الموظفين", EmployeeGrid()));
        return Scroll(page);
    }

    private UIElement AddEmployee()
    {
        _pageTitle.Text = "إضافة موظف";
        var page = Page();
        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition());
        grid.ColumnDefinitions.Add(new ColumnDefinition());
        var fields = new (string, string)[] {
            ("الرقم الوظيفي", "أدخل الرقم الوظيفي"), ("الاسم الكامل", "أدخل اسم الموظف"),
            ("القسم", "اختر القسم"), ("المنصب", "أدخل المنصب"),
            ("تاريخ التعيين", "2026-06-09"), ("الرصيد السنوي", "30") };
        for (var i = 0; i < fields.Length; i++)
        {
            var stack = new StackPanel { Margin = new Thickness(i % 2 == 0 ? 8 : 0, 0, i % 2 == 0 ? 0 : 8, 14) };
            stack.Children.Add(Label(fields[i].Item1));
            stack.Children.Add(Input(fields[i].Item2));
            Grid.SetColumn(stack, i % 2);
            Grid.SetRow(stack, i / 2);
            if (grid.RowDefinitions.Count <= i / 2) grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            grid.Children.Add(stack);
        }
        var actions = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(0, 12, 0, 0) };
        actions.Children.Add(Button("حفظ", Primary, Brushes.White, 42, 120));
        var cancel = Button("إلغاء", Brushes.White, TextPrimary, 42, 110);
        cancel.Click += (_, _) => Navigate("الموظفون");
        actions.Children.Add(cancel);
        page.Children.Add(Section("البيانات الأساسية", grid));
        page.Children.Add(actions);
        return Scroll(page);
    }

    private UIElement RegularLeave() => LeaveForm("طلب إجازة اعتيادية", new[] { "الموظف", "تاريخ البداية", "تاريخ النهاية", "السبب" },
        new[] { ("عدد الأيام", "6", Primary), ("الرصيد الحالي", "20", Success), ("الرصيد بعد الخصم", "14", Warning) });

    private UIElement SickLeave()
    {
        var page = Page();
        page.Children.Add(FormGrid(new[] { "الموظف", "التاريخ", "عدد الأيام", "الجهة الصحية" }));
        page.Children.Add(UploadBox());
        page.Children.Add(ActionBar());
        return Scroll(page);
    }

    private UIElement HourlyLeave() => LeaveForm("طلب إجازة زمنية", new[] { "الموظف", "التاريخ", "وقت الخروج", "وقت العودة", "السبب" },
        new[] { ("عدد الساعات", "4", Primary) });

    private UIElement Approvals()
    {
        var page = Page();
        var tabs = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 0, 0, 14) };
        foreach (var tab in new[] { "الكل", "إجازات اعتيادية", "إجازات مرضية", "إجازات زمنية" })
            tabs.Children.Add(Button(tab, tab == "الكل" ? Primary : Brushes.White, tab == "الكل" ? Brushes.White : TextPrimary, 36, 130));
        page.Children.Add(tabs);
        var grid = new UniformGrid { Columns = 3 };
        foreach (var a in new[] { ("أحمد السعيد", "إجازة اعتيادية", "2026-06-11 إلى 2026-06-16", "6 أيام"), ("سارة أحمد", "إجازة مرضية", "2026-06-09", "2 أيام"), ("محمد علي", "إجازة زمنية", "09:00 إلى 13:00", "4 ساعات") })
            grid.Children.Add(ApprovalCard(a.Item1, a.Item2, a.Item3, a.Item4));
        page.Children.Add(grid);
        return Scroll(page);
    }

    private UIElement Reports()
    {
        var page = Page();
        var grid = new UniformGrid { Columns = 3 };
        foreach (var r in new[] { "تقرير موظف", "تقرير قسم", "تقرير الأرصدة", "تقرير المرضيات", "تقرير الزمنيات", "التقرير الشهري" })
            grid.Children.Add(ReportCard(r));
        page.Children.Add(grid);
        return Scroll(page);
    }

    private UIElement Settings()
    {
        var page = Page();
        page.Children.Add(Section("إعدادات عامة", FormGrid(new[] { "اسم المؤسسة", "الشعار", "بداية السنة المالية", "عدد أيام الإجازة السنوية" })));
        page.Children.Add(Section("إعدادات الإشعارات", new StackPanel
        {
            Children =
            {
                Check("تفعيل الإشعارات", new Thickness(0,0,0,10)),
                Check("إشعارات البريد الإلكتروني")
            }
        }));
        page.Children.Add(ActionBar("حفظ التعديلات"));
        return Scroll(page);
    }

    private UIElement LeaveForm(string title, string[] fields, (string title, string value, Brush color)[] totals)
    {
        var page = Page();
        page.Children.Add(Section(title, FormGrid(fields)));
        var grid = new UniformGrid { Columns = totals.Length, Margin = new Thickness(0, 4, 0, 16) };
        foreach (var t in totals) grid.Children.Add(StatCard("", t.value, t.title, t.color));
        page.Children.Add(grid);
        page.Children.Add(ActionBar());
        return Scroll(page);
    }

    private StackPanel Page() => new() { FlowDirection = FlowDirection.RightToLeft };
    private ScrollViewer Scroll(UIElement child) => new() { Content = child, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };

    private Border Card(Thickness margin, double radius = 8) => new()
    {
        Background = CardBrush,
        BorderBrush = BorderBrushSoft,
        BorderThickness = new Thickness(1),
        CornerRadius = new CornerRadius(radius),
        Effect = (System.Windows.Media.Effects.Effect)Resources["SoftShadow"],
        Margin = margin
    };

    private UIElement Section(string title, UIElement content)
    {
        var card = Card(new Thickness(0, 0, 0, 16), 12);
        var stack = new StackPanel { Margin = new Thickness(18) };
        stack.Children.Add(Text(title, 18, FontWeights.Bold, TextPrimary, margin: new Thickness(0, 0, 0, 14)));
        stack.Children.Add(content);
        card.Child = stack;
        return card;
    }

    private Border StatCard(string icon, string value, string title, Brush color)
    {
        var card = Card(new Thickness(6), 16);
        card.Height = 120;
        var stack = new StackPanel { Margin = new Thickness(18), VerticalAlignment = VerticalAlignment.Center };
        if (!string.IsNullOrWhiteSpace(icon)) stack.Children.Add(Text(icon, 24, FontWeights.Bold, color));
        stack.Children.Add(Text(value, 28, FontWeights.Bold, color, margin: new Thickness(0, 2, 0, 0)));
        stack.Children.Add(Text(title, 13, FontWeights.Medium, TextSecondary));
        card.Child = stack;
        return card;
    }

    private UIElement FormGrid(string[] fields)
    {
        var grid = new Grid();
        grid.ColumnDefinitions.Add(new ColumnDefinition());
        grid.ColumnDefinitions.Add(new ColumnDefinition());
        for (var i = 0; i < fields.Length; i++)
        {
            if (grid.RowDefinitions.Count <= i / 2) grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            var stack = new StackPanel { Margin = new Thickness(8, 0, 8, 14) };
            stack.Children.Add(Label(fields[i]));
            stack.Children.Add(fields[i].Contains("القسم") || fields[i] == "الموظف" ? Combo("اختر " + fields[i], "أحمد محمد", "سارة أحمد", "محمد علي") : Input("أدخل " + fields[i]));
            Grid.SetColumn(stack, i % 2);
            Grid.SetRow(stack, i / 2);
            grid.Children.Add(stack);
        }
        return grid;
    }

    private UIElement ActionBar(string saveText = "حفظ الطلب")
    {
        var actions = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Left };
        actions.Children.Add(Button(saveText, Primary, Brushes.White, 44, 150));
        actions.Children.Add(Button("إلغاء", Brushes.White, TextPrimary, 44, 100));
        return actions;
    }

    private UIElement UploadBox()
    {
        var card = new Border
        {
            BorderBrush = Primary,
            BorderThickness = new Thickness(1),
            CornerRadius = new CornerRadius(8),
            Height = 130,
            Margin = new Thickness(8, 4, 8, 18),
            Background = Brush("#EFF6FF")
        };
        var stack = new StackPanel { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center };
        stack.Children.Add(Text("إرفاق التقرير الطبي", 15, FontWeights.Bold, Primary, HorizontalAlignment.Center));
        stack.Children.Add(Text("اسحب الملف هنا أو اضغط للاختيار", 13, FontWeights.Medium, TextSecondary, HorizontalAlignment.Center, margin: new Thickness(0, 8, 0, 0)));
        card.Child = stack;
        return card;
    }

    private DataGrid EmployeeGrid()
    {
        var rows = new ObservableCollection<Employee>
        {
            new("1001", "أحمد محمد", "تقنية المعلومات", "مدير فريق", 30, 10, 20, "نشط"),
            new("1002", "سارة أحمد", "الموارد البشرية", "أخصائي موارد", 30, 5, 25, "نشط"),
            new("1003", "محمد علي", "المالية", "محاسب", 30, 12, 18, "نشط"),
            new("1004", "مريم حسن", "التسويق", "أخصائي تسويق", 30, 8, 22, "نشط")
        };
        var table = DataTableGrid(rows);
        table.Columns.Add(TextColumn("الرقم الوظيفي", "الرقم_الوظيفي", 120));
        table.Columns.Add(TextColumn("الاسم", "الاسم", 170));
        table.Columns.Add(TextColumn("القسم", "القسم", 160));
        table.Columns.Add(TextColumn("المنصب", "المنصب", 150));
        table.Columns.Add(TextColumn("الرصيد السنوي", "الرصيد_السنوي", 120));
        table.Columns.Add(TextColumn("المستهلك", "المستهلك", 100));
        table.Columns.Add(TextColumn("المتبقي", "المتبقي", 100));
        table.Columns.Add(TextColumn("الحالة", "الحالة", 90));
        return table;
    }

    private DataGrid RequestGrid()
    {
        var rows = new ObservableCollection<Request>
        {
            new("أحمد السعيد", "إجازة اعتيادية", "2026-06-11", "6 أيام", "معلّق"),
            new("سارة أحمد", "إجازة مرضية", "2026-06-09", "2 أيام", "معلّق"),
            new("محمد علي", "إجازة زمنية", "2026-06-10", "4 ساعات", "معلّق")
        };
        var table = DataTableGrid(rows);
        table.Columns.Add(TextColumn("الموظف", "الموظف", 180));
        table.Columns.Add(TextColumn("نوع الإجازة", "نوع_الإجازة", 160));
        table.Columns.Add(TextColumn("التاريخ", "التاريخ", 130));
        table.Columns.Add(TextColumn("المدة", "المدة", 110));
        table.Columns.Add(TextColumn("الحالة", "الحالة", 110));
        return table;
    }

    private DataGrid DataTableGrid<T>(IEnumerable<T> rows) => new()
    {
        ItemsSource = rows,
        AutoGenerateColumns = false,
        CanUserAddRows = false,
        IsReadOnly = true,
        RowHeight = 48,
        HeadersVisibility = DataGridHeadersVisibility.Column,
        GridLinesVisibility = DataGridGridLinesVisibility.Horizontal,
        BorderThickness = new Thickness(0),
        Background = CardBrush,
        RowBackground = CardBrush,
        AlternatingRowBackground = Brush("#F8FAFC"),
        HorizontalGridLinesBrush = BorderBrushSoft,
        ColumnHeaderHeight = 46,
        FontSize = 13,
        FlowDirection = FlowDirection.RightToLeft
    };

    private DataGridTextColumn TextColumn(string header, string path, double width)
    {
        var textStyle = new Style(typeof(TextBlock));
        textStyle.Setters.Add(new Setter(TextBlock.TextAlignmentProperty, TextAlignment.Right));
        textStyle.Setters.Add(new Setter(FrameworkElement.FlowDirectionProperty, FlowDirection.RightToLeft));
        textStyle.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Center));
        textStyle.Setters.Add(new Setter(FrameworkElement.MarginProperty, new Thickness(12, 0, 12, 0)));

        return new DataGridTextColumn
        {
            Header = header,
            Binding = new Binding(path),
            Width = new DataGridLength(width),
            ElementStyle = textStyle
        };
    }

    private Border ApprovalCard(string name, string type, string period, string duration)
    {
        var card = Card(new Thickness(6), 12);
        var stack = new StackPanel { Margin = new Thickness(16) };
        stack.Children.Add(Text(name, 18, FontWeights.Bold, TextPrimary));
        stack.Children.Add(Text(type, 13, FontWeights.Medium, Warning, margin: new Thickness(0, 6, 0, 0)));
        stack.Children.Add(Text(period, 13, FontWeights.Medium, TextSecondary, margin: new Thickness(0, 10, 0, 0)));
        stack.Children.Add(Text(duration, 13, FontWeights.Bold, Success, margin: new Thickness(0, 6, 0, 14)));
        var actions = new StackPanel { Orientation = Orientation.Horizontal };
        actions.Children.Add(Button("موافقة", Success, Brushes.White, 34, 82));
        actions.Children.Add(Button("رفض", Brush("#FEE2E2"), Danger, 34, 72));
        actions.Children.Add(Button("عرض التفاصيل", Brushes.White, Primary, 34, 110));
        stack.Children.Add(actions);
        card.Child = stack;
        return card;
    }

    private Border ReportCard(string title)
    {
        var card = Card(new Thickness(8), 12);
        var stack = new StackPanel { Margin = new Thickness(18) };
        stack.Children.Add(Text("📊", 28, FontWeights.Bold, Primary));
        stack.Children.Add(Text(title, 18, FontWeights.Bold, TextPrimary, margin: new Thickness(0, 10, 0, 0)));
        stack.Children.Add(Text("تقرير جاهز وسريع للعرض والطباعة", 13, FontWeights.Medium, TextSecondary, margin: new Thickness(0, 6, 0, 16)));
        stack.Children.Add(Button("فتح التقرير", Brushes.White, Primary, 38, 130));
        card.Child = stack;
        return card;
    }

    private StackPanel NavContent(string icon, string title)
    {
        var stack = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            FlowDirection = FlowDirection.RightToLeft,
            HorizontalAlignment = HorizontalAlignment.Right
        };
        stack.Children.Add(Text(icon, 14, FontWeights.Bold, Brushes.White, VerticalAlignment.Center, margin: new Thickness(0, 0, 0, 0)));
        stack.Children.Add(Text(title, 13, FontWeights.Bold, Brushes.White, VerticalAlignment.Center, margin: new Thickness(8, 0, 0, 0)));
        return stack;
    }

    private TextBlock Text(string text, double size, FontWeight weight, Brush brush, HorizontalAlignment align = HorizontalAlignment.Stretch, Thickness? margin = null) =>
        new() { Text = text, FontSize = size, FontWeight = weight, Foreground = brush, HorizontalAlignment = align, Margin = margin ?? new Thickness(0), TextWrapping = TextWrapping.Wrap };

    private TextBlock Text(string text, double size, FontWeight weight, Brush brush, VerticalAlignment verticalAlignment, Thickness? margin = null) =>
        new() { Text = text, FontSize = size, FontWeight = weight, Foreground = brush, VerticalAlignment = verticalAlignment, Margin = margin ?? new Thickness(0), TextWrapping = TextWrapping.Wrap };

    private TextBlock Label(string text, Thickness? margin = null) =>
        Text(text, 13, FontWeights.Bold, TextPrimary, margin: margin ?? new Thickness(0, 0, 0, 6));

    private TextBox Input(string placeholder) => new()
    {
        Height = 42,
        Tag = placeholder,
        Text = string.Empty,
        Foreground = TextPrimary,
        Background = Brushes.White,
        BorderBrush = BorderBrushSoft,
        BorderThickness = new Thickness(1),
        Padding = new Thickness(12, 0, 12, 0),
        VerticalContentAlignment = VerticalAlignment.Center
    };

    private ComboBox Combo(string selected, params string[] items)
    {
        var combo = new ComboBox { Height = 42, Margin = new Thickness(8, 0, 8, 0), BorderBrush = BorderBrushSoft, Padding = new Thickness(10, 0, 10, 0), VerticalContentAlignment = VerticalAlignment.Center };
        combo.Items.Add(selected);
        foreach (var item in items) combo.Items.Add(item);
        combo.SelectedIndex = 0;
        return combo;
    }

    private CheckBox Check(string text, Thickness? margin = null) => new()
    {
        Content = text,
        Foreground = TextPrimary,
        FontWeight = FontWeights.Medium,
        Margin = margin ?? new Thickness(0)
    };

    private Button Button(string text, Brush background, Brush foreground, double height, double width = double.NaN) => new()
    {
        Content = text,
        Height = height,
        Width = width,
        Margin = new Thickness(6, 0, 6, 0),
        Padding = new Thickness(14, 0, 14, 0),
        Background = background,
        Foreground = foreground,
        BorderBrush = background == Brushes.White ? BorderBrushSoft : background,
        BorderThickness = new Thickness(1),
        FontWeight = FontWeights.Bold,
        Cursor = System.Windows.Input.Cursors.Hand
    };

    private BitmapImage Logo() => new(new Uri("pack://application:,,,/Assets/LOGO.png"));
}

public record Employee(string الرقم_الوظيفي, string الاسم, string القسم, string المنصب, int الرصيد_السنوي, int المستهلك, int المتبقي, string الحالة);
public record Request(string الموظف, string نوع_الإجازة, string التاريخ, string المدة, string الحالة);
