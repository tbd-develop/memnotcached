public record Command(
    string Name,
    string Key,
    string Flags = "",
    string Exptime = "",
    string Bytes = "",
    bool Noreply = false
);