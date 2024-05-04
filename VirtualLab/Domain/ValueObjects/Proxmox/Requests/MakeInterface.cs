namespace VirtualLab.Domain.Value_Objects;

public record CreateInterface
{ // по сути все эти данные хранярся в nets если смысл их отдельно делать этим классам?
    public string IFace { get; set; }
    public string Node { get; set; }
    public string Type { get; set; }


    public static CreateInterface Brige(string iFace, string node)
    {
        return new CreateInterface()
        {
            IFace = iFace,
            Node = node,
            Type = "bridge"
        };
    }
}