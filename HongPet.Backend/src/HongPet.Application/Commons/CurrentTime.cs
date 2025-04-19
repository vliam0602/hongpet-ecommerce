namespace HongPet.Application.Commons;
public class CurrentTime
{
    public static DateTime GetCurrentTime => DateTime.UtcNow.ToLocalTime();
}
