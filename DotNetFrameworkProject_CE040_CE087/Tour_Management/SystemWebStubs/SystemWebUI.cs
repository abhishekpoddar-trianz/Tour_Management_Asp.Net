using System;

namespace System.Web.UI.HtmlControls
{
    public class HtmlForm : System.Web.UI.Control
    {
    }

    public class HtmlGenericControl : System.Web.UI.Control
    {
        public HtmlGenericControl() { }
        public HtmlGenericControl(string tag) { }
    }

    public class HtmlInputControl : System.Web.UI.Control
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class HtmlInputText : HtmlInputControl
    {
        public HtmlInputText() { Type = "text"; }
    }

    public class HtmlInputPassword : HtmlInputControl
    {
        public HtmlInputPassword() { Type = "password"; }
    }

    public class HtmlInputFile : HtmlInputControl
    {
        public HtmlInputFile() { Type = "file"; }
        public HttpPostedFile PostedFile => new HttpPostedFile();
    }

    public class HtmlInputButton : HtmlInputControl
    {
        public HtmlInputButton() { Type = "button"; }
    }

    public class HtmlButton : System.Web.UI.Control
    {
    }

    public class HtmlAnchor : System.Web.UI.Control
    {
        public string HRef { get; set; }
    }

    public class HtmlImage : System.Web.UI.Control
    {
        public string Src { get; set; }
    }

    public class HtmlTable : System.Web.UI.Control
    {
    }

    public class HtmlTableRow : System.Web.UI.Control
    {
    }

    public class HtmlTableCell : System.Web.UI.Control
    {
    }
}

namespace System.Web.UI
{
    public class Page
    {
        public HttpRequest Request => new HttpRequest();
        public HttpResponse Response => new HttpResponse();
        public HttpSessionState Session => new HttpSessionState();
        public HttpServerUtility Server => new HttpServerUtility();

        protected virtual void Page_Load(object sender, EventArgs e) { }

        public bool IsPostBack { get; set; }
    }

    public class UserControl
    {
        public HttpRequest Request => new HttpRequest();
        public HttpResponse Response => new HttpResponse();
        public HttpSessionState Session => new HttpSessionState();
    }

    public class Control
    {
        public virtual string ID { get; set; }
        public Control FindControl(string id) => null;
    }

    public class HtmlForm : Control
    {
    }
}

namespace System.Web.UI.WebControls
{
    public class TextBox : System.Web.UI.Control
    {
        public string Text { get; set; }
        public TextMode TextMode { get; set; }
    }

    public enum TextMode
    {
        SingleLine,
        MultiLine,
        Password
    }

    public class Button : System.Web.UI.Control
    {
        public string Text { get; set; }
        public event EventHandler Click;
    }

    public class Label : System.Web.UI.Control
    {
        public string Text { get; set; }
    }

    public class LinkButton : System.Web.UI.Control
    {
        public string Text { get; set; }
        public event EventHandler Click;
    }

    public class HyperLink : System.Web.UI.Control
    {
        public string NavigateUrl { get; set; }
        public string Text { get; set; }
    }

    public class GridView : System.Web.UI.Control
    {
        public object DataSource { get; set; }
        public string[] DataKeyNames { get; set; }
        public bool AutoGenerateColumns { get; set; }
        public GridViewRowCollection Rows { get; set; }
        public event GridViewDeleteEventHandler RowDeleting;
        public event GridViewEditEventHandler RowEditing;
        public event GridViewCancelEditEventHandler RowCancelingEdit;
        public event GridViewUpdateEventHandler RowUpdating;
        public event GridViewPageEventHandler PageIndexChanging;
        public int EditIndex { get; set; }
        public int PageIndex { get; set; }

        public void DataBind() { }
    }

    public class GridViewRowCollection
    {
        public int Count => 0;
        public GridViewRow this[int index] => new GridViewRow(0, 0, DataControlRowType.DataRow, DataControlRowState.Normal);
    }

    public class GridViewRow
    {
        public GridViewRow(int rowIndex, int dataItemIndex, DataControlRowType rowType, DataControlRowState rowState)
        {
            RowIndex = rowIndex;
            DataItemIndex = dataItemIndex;
            RowType = rowType;
            RowState = rowState;
        }

        public int RowIndex { get; set; }
        public int DataItemIndex { get; set; }
        public DataControlRowType RowType { get; set; }
        public DataControlRowState RowState { get; set; }
        public TableCellCollection Cells => new TableCellCollection();
    }

    public class TableCellCollection
    {
        public TableCell this[int index] => new TableCell();
    }

    public class TableCell
    {
        public ControlCollection Controls => new ControlCollection();
    }

    public class ControlCollection
    {
        public System.Web.UI.Control this[int index] => new System.Web.UI.Control();
    }

    public enum DataControlRowType
    {
        DataRow,
        Header,
        Footer
    }

    public enum DataControlRowState
    {
        Normal,
        Edit
    }

    public delegate void GridViewDeleteEventHandler(object sender, GridViewDeleteEventArgs e);
    public delegate void GridViewEditEventHandler(object sender, GridViewEditEventArgs e);
    public delegate void GridViewCancelEditEventHandler(object sender, GridViewCancelEditEventArgs e);
    public delegate void GridViewUpdateEventHandler(object sender, GridViewUpdateEventArgs e);
    public delegate void GridViewPageEventHandler(object sender, GridViewPageEventArgs e);

    public class GridViewDeleteEventArgs : EventArgs
    {
        public int RowIndex { get; set; }
        public bool Cancel { get; set; }
    }

    public class GridViewEditEventArgs : EventArgs
    {
        public int NewEditIndex { get; set; }
        public bool Cancel { get; set; }
    }

    public class GridViewCancelEditEventArgs : EventArgs
    {
        public int RowIndex { get; set; }
        public bool Cancel { get; set; }
    }

    public class GridViewUpdateEventArgs : EventArgs
    {
        public int RowIndex { get; set; }
        public bool Cancel { get; set; }
    }

    public class GridViewPageEventArgs : EventArgs
    {
        public int NewPageIndex { get; set; }
        public bool Cancel { get; set; }
    }

    public class DropDownList : System.Web.UI.Control
    {
        public string SelectedValue { get; set; }
        public string Text { get; set; }
        public ListItemCollection Items { get; set; }
        public void DataBind() { }
    }

    public class ListItemCollection
    {
        public void Add(string text) { }
        public void Clear() { }
    }

    public class FileUpload : System.Web.UI.Control
    {
        public bool HasFile => false;
        public string FileName => string.Empty;
        public void SaveAs(string filename) { }
    }

    public class Literal : System.Web.UI.Control
    {
        public string Text { get; set; }
    }

    public class Image : System.Web.UI.Control
    {
        public string ImageUrl { get; set; }
    }

    public class CheckBox : System.Web.UI.Control
    {
        public bool Checked { get; set; }
    }

    public class RadioButton : System.Web.UI.Control
    {
        public bool Checked { get; set; }
        public string GroupName { get; set; }
    }

    public class SqlDataSource : System.Web.UI.Control
    {
        public string ConnectionString { get; set; }
        public string SelectCommand { get; set; }
        public string InsertCommand { get; set; }
        public string UpdateCommand { get; set; }
        public string DeleteCommand { get; set; }
    }

    public class RegularExpressionValidator : System.Web.UI.Control
    {
        public string ControlToValidate { get; set; }
        public string ErrorMessage { get; set; }
        public string ValidationExpression { get; set; }
        public bool IsValid { get; set; }
    }

    public class RequiredFieldValidator : System.Web.UI.Control
    {
        public string ControlToValidate { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsValid { get; set; }
    }

    public class CompareValidator : System.Web.UI.Control
    {
        public string ControlToValidate { get; set; }
        public string ControlToCompare { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsValid { get; set; }
    }

    public class RangeValidator : System.Web.UI.Control
    {
        public string ControlToValidate { get; set; }
        public string ErrorMessage { get; set; }
        public string MinimumValue { get; set; }
        public string MaximumValue { get; set; }
        public bool IsValid { get; set; }
    }

    public class ValidationSummary : System.Web.UI.Control
    {
    }
}

namespace System.Web
{
    public class HttpRequest
    {
        public string this[string key] => string.Empty;
        public HttpFileCollection Files => new HttpFileCollection();
    }

    public class HttpResponse
    {
        public void Redirect(string url) { }
        public void Write(string s) { }
    }

    public class HttpSessionState
    {
        public object this[string key]
        {
            get => null;
            set { }
        }
    }

    public class HttpServerUtility
    {
        public string MapPath(string path) => path;
        public void Transfer(string path) { }
        public void Transfer(string path, bool preserveForm) { }
    }

    public class HttpFileCollection
    {
        public int Count => 0;
        public HttpPostedFile this[int index] => null;
    }

    public class HttpPostedFile
    {
        public string FileName => string.Empty;
        public void SaveAs(string filename) { }
    }
}
