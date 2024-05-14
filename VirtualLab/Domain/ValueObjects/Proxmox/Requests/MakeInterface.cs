namespace VirtualLab.Domain.Value_Objects;

public record CreateInterface // todo: замеить это на net. тогда по всему коду будут две сущности в виде net и vm и ваще тооопппп.
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