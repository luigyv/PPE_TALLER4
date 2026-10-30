using JuegosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace JuegosApi.Data;

// Datos de demostración: los mismos 13 juegos del mock del cliente Vue.
// Solo corre si la tabla está vacía y "Seed:Juegos" es true (appsettings.Development.json).
public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext db)
    {
        if (await db.Juegos.AnyAsync()) return;

        var datos = new (string Nombre, string Imagen, string Categoria)[]
        {
        ("Hollow Knight", "https://upload.wikimedia.org/wikipedia/en/d/de/Hollow_Knight_2026_cover_art.jpg?utm_source=en.wikipedia.org&utm_campaign=index&utm_content=original", "Aventura"),
        ("Celeste", "https://assets.nintendo.com/image/upload/q_auto/f_auto/store/software/switch/70010000006442/691ba3e0801180a9864cc8a7694b6f98097f9d9799bc7e3dc6db92f086759252", "Plataformas"),
        ("FIFA 22", "https://media.gq.com.mx/photos/60e9cc9c33c54bdef6761922/1:1/w_2125,h_2125,c_limit/E53RdQ5WQAIs7HC-scaled.jpg", "Deportes"),
        ("Civilization VI", "https://assets.nintendo.com/image/upload/c_fill,w_1200/q_auto:best/f_auto/dpr_2.0/store/software/switch/70010000013704/918c0badde3aeba760e2185f382a2402248a1292322cf540fd8d098eeb292e1e", "Estrategia"),
        ("Doom Eternal", "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/782330/3efb12f884eaee39b4853721f5cf1c6e2b706495/capsule_616x353.jpg?t=1783432602", "Acción"),
        ("Stardew Valley", "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/413150/capsule_616x353.jpg?t=1786554168", "Simulación"),
        ("League of Legends", "https://cdn1.epicgames.com/offer/24b9b5e323bc40eea252a10cdd3b2f10/EGS_LeagueofLegends_RiotGames_S1_2560x1440-47eb328eac5ddd63ebd096ded7d0d5ab", "Estrategia"),
        ("God of War Ragnarök", "https://i.blogs.es/eadb27/1366_2000/1200_630.jpeg", "Acción"),
        ("Among Us", "https://assets.nintendo.com/image/upload/ar_16:9,c_lpad,w_1240/b_white/f_auto/q_auto/store/software/switch/70010000036098/758ab0b61205081da2466386940752c70e0e5ea43bd39e8b9b13eaa455c69b7e", "Fiesta"),
        ("The Sims 4", "https://shared.fastly.steamstatic.com/store_item_assets/steam/apps/1222670/capsule_616x353.jpg?t=1787850077", "Simulación"),
        ("Fortnite", "https://cms-assets.unrealengine.com/cm6l5gfpm05kr07my04cqgy2x/cmt14h2ardvpq07o76t76zt0m", "Acción"),
        ("Minecraft", "https://www.nintendo.com/eu/media/images/news_12/2026/march_42/tiny_takeover_drop_is_now_available_for_minecraft_on_nintendo_switch/16x9_MinecraftTinyTakeover_Textless.png", "Aventura"),
        ("Hollow Knight: Silksong", "https://assets.nintendo.com/image/upload/ar_16:9,c_lpad,w_1240/b_white/f_auto/q_auto/store/software/switch2/70010000105851/8787627be7f26ae7984456ffd9af17bea845032cebbf59fe6eeb596dea6bb20e", "Aventura")
        };

        var ahora = DateTime.UtcNow;
        db.Juegos.AddRange(datos.Select(d => new Juego
        {
            Nombre = d.Nombre,
            Imagen = d.Imagen,
            Categoria = d.Categoria,
            CreadoEn = ahora
        }));

        await db.SaveChangesAsync();
    }
}
