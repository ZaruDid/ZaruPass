namespace ZaruPass.Core.DataBase.Migrations;

public partial class Password
{
    public int Id { get; set; }

    public string Site { get; set; } = null!;

    public byte[] Pass { get; set; } = null!;
}
