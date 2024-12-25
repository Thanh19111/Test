public void LoadData()
{
    string link = "http://localhost:91/testpool1/api/sanpham";
    HttpWebRequest req = WebRequest.CreateHttp(link);
    WebResponse res = req.GetResponse();
    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(SanPham[]));
    object data = js.ReadObject(res.GetResponseStream());
    SanPham[] arr = data as SanPham[];
    datagrid.DataSource = arr;
}
public void LoadCBO()
{
    string link = "http://localhost:91/testpool1/api/danhmuc";
    HttpWebRequest req = WebRequest.CreateHttp(link);
    WebResponse res = req.GetResponse();
    DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(DanhMuc[]));
    object data = js.ReadObject(res.GetResponseStream());
    DanhMuc[] arr = data as DanhMuc[];
    cbo.DataSource = arr;
    cbo.DisplayMember = "TenDanhMuc";
    cbo.ValueMember = "MaDanhMuc";
}

private void button1_Click(object sender, EventArgs e)
{
    try
    {
        //them
        int ma = int.Parse(txtma.Text.Trim());
        string ten = txtten.Text.Trim();
        int dongia = int.Parse(txtdg.Text.Trim());
        string postString = String.Format("?ma={0}&ten={1}&dongia={2}&madm={3}", ma, ten, dongia, cbo.SelectedValue);
        string link = "http://localhost:91/testpool1/api/sanpham/" + postString;
        HttpWebRequest req = WebRequest.CreateHttp(link);
        req.Method = "POST";
        Stream datastream = req.GetRequestStream();
        WebResponse res = req.GetResponse();
        DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(bool));
        object data = js.ReadObject(res.GetResponseStream());
        bool kq = (bool)data;
        if(kq)
        {
            MessageBox.Show("Thêm sản phẩm thành công");
            LoadData();
        }else
        {
            MessageBox.Show("Thêm sản phẩm thất bại");
        }
    }
    catch(Exception ex)
    {
        MessageBox.Show("Lỗi khi nhập dữ liệu " + ex.Message);
    }
    
}
--------------------------------------------------------------------------
CSDLTestEntities db = new CSDLTestEntities();
[HttpGet]
public List<SanPham> laySanPham()
{
    return db.SanPhams.ToList();
}

[HttpGet]
public List<SanPham> LaySanPhamTheoDM(int madm)
{
    return db.SanPhams.Where(x =>x.MaDanhMuc == madm).ToList();
}

[HttpPost]
public bool Them(int ma, string ten, int dongia, int madm)
{
    var sp = db.SanPhams.FirstOrDefault(x=>x.Ma == ma);
    if (sp != null)
    {
        return false;
    }else
    {
        SanPham sp1 = new SanPham();
        sp1.Ma = ma;
        sp1.Ten = ten;
        sp1.DonGia = dongia;
        sp1.MaDanhMuc = madm;
        db.SanPhams.Add(sp1);
        db.SaveChanges();
        return true;
    }
}

[HttpPut]
public bool Sua(int ma, string ten, int dongia, int madm)
{
    var sp = db.SanPhams.FirstOrDefault(x => x.Ma == ma);
    if (sp == null)
    {
        return false;
    }
    else
    {
        sp.Ma = ma;
        sp.Ten = ten;
        sp.DonGia = dongia;
        sp.MaDanhMuc = madm;
        db.SaveChanges();
        return true;
    }
}

[HttpDelete]
public bool Xoa(int ma)
{
    var sp = db.SanPhams.FirstOrDefault(x => x.Ma == ma);
    if (sp == null)
    {
        return false;
    }
    else
    {
        db.SanPhams.Remove(sp);
        db.SaveChanges();
        return true;
    }
}