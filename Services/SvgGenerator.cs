namespace SocialApp.Services
{
    public static class SvgGenerator
    {
        public static void CreateAllImages(IWebHostEnvironment env)
        {
            var avatarsPath = Path.Combine(env.WebRootPath, "images", "avatars");
            var postsPath = Path.Combine(env.WebRootPath, "images", "posts");
            
            // Создаем папки
            Directory.CreateDirectory(avatarsPath);
            Directory.CreateDirectory(postsPath);
            
            // Default avatar
            var defaultAvatar = Path.Combine(env.WebRootPath, "images", "default-avatar.svg");
            if (!File.Exists(defaultAvatar))
            {
                File.WriteAllText(defaultAvatar, GetDefaultAvatarSvg());
                Console.WriteLine("✅ Created default-avatar.svg");
            }
            
            // Аватарки пользователей (5 штук)
            for (int i = 1; i <= 5; i++)
            {
                var avatarPath = Path.Combine(avatarsPath, $"avatar{i}.svg");
                if (!File.Exists(avatarPath))
                {
                    File.WriteAllText(avatarPath, GetAvatarSvg(i));
                    Console.WriteLine($"✅ Created avatar{i}.svg");
                }
            }
            
            // Изображения для постов
            CreatePostImage(postsPath, "post1.svg", GetNatureSvg());
            CreatePostImage(postsPath, "post2.svg", GetMountainsSvg());
            CreatePostImage(postsPath, "post3.svg", GetFoodSvg());
            CreatePostImage(postsPath, "post4.svg", GetTechSvg());
            CreatePostImage(postsPath, "post5.svg", GetArtSvg());
        }

        private static void CreatePostImage(string path, string filename, string svgContent)
        {
            var filePath = Path.Combine(path, filename);
            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, svgContent);
                Console.WriteLine($"✅ Created {filename}");
            }
        }

        private static string GetDefaultAvatarSvg() => @"<svg xmlns='http://www.w3.org/2000/svg' width='200' height='200' viewBox='0 0 200 200'>
  <circle cx='100' cy='100' r='100' fill='#607D8B'/>
  <circle cx='100' cy='80' r='25' fill='#CFD8DC'/>
  <circle cx='80' cy='75' r='5' fill='#37474F'/>
  <circle cx='120' cy='75' r='5' fill='#37474F'/>
  <path d='M 70 120 Q 100 140 130 120' stroke='#CFD8DC' stroke-width='4' fill='none' stroke-linecap='round'/>
</svg>";

        private static string GetAvatarSvg(int index)
        {
            string[] colors = { "#4CAF50", "#2196F3", "#FF9800", "#9C27B0", "#F44336" };
            string color = colors[(index - 1) % colors.Length];
            
            return $@"<svg xmlns='http://www.w3.org/2000/svg' width='200' height='200' viewBox='0 0 200 200'>
  <circle cx='100' cy='100' r='100' fill='{color}'/>
  <circle cx='100' cy='80' r='25' fill='white'/>
  <circle cx='80' cy='75' r='5' fill='#333'/>
  <circle cx='120' cy='75' r='5' fill='#333'/>
  <path d='M 70 120 Q 100 140 130 120' stroke='white' stroke-width='4' fill='none' stroke-linecap='round'/>
</svg>";
        }

        private static string GetNatureSvg() => @"<svg xmlns='http://www.w3.org/2000/svg' width='800' height='600' viewBox='0 0 800 600'>
  <rect width='800' height='600' fill='#87CEEB'/>
  <rect x='0' y='400' width='800' height='200' fill='#8BC34A'/>
  <circle cx='400' cy='300' r='80' fill='#FFC107'/>
  <circle cx='200' cy='250' r='40' fill='#4CAF50'/>
  <circle cx='600' cy='280' r='50' fill='#388E3C'/>
  <text x='400' y='550' text-anchor='middle' fill='white' font-size='24' font-family='Arial'>🌳 Природа 🌳</text>
</svg>";

        private static string GetMountainsSvg() => @"<svg xmlns='http://www.w3.org/2000/svg' width='800' height='600' viewBox='0 0 800 600'>
  <rect width='800' height='600' fill='#4FC3F7'/>
  <polygon points='400,100 150,450 650,450' fill='#8BC34A'/>
  <polygon points='400,150 250,400 550,400' fill='#689F38'/>
  <rect x='0' y='450' width='800' height='150' fill='#388E3C'/>
  <text x='400' y='550' text-anchor='middle' fill='white' font-size='24' font-family='Arial'>⛰️ Горы ⛰️</text>
</svg>";

        private static string GetFoodSvg() => @"<svg xmlns='http://www.w3.org/2000/svg' width='800' height='600' viewBox='0 0 800 600'>
  <rect width='800' height='600' fill='#FFF8E1'/>
  <ellipse cx='400' cy='350' rx='180' ry='120' fill='#FF9800'/>
  <ellipse cx='400' cy='320' rx='150' ry='90' fill='#F44336'/>
  <text x='400' y='550' text-anchor='middle' fill='#333' font-size='24' font-family='Arial'>🍝 Еда 🍝</text>
</svg>";

        private static string GetTechSvg() => @"<svg xmlns='http://www.w3.org/2000/svg' width='800' height='600' viewBox='0 0 800 600'>
  <rect width='800' height='600' fill='#1a1a2e'/>
  <rect x='250' y='200' width='300' height='200' rx='10' fill='#16213e' stroke='#0f3460' stroke-width='3'/>
  <rect x='280' y='230' width='240' height='120' fill='#0f3460'/>
  <rect x='350' y='400' width='100' height='80' fill='#16213e' stroke='#0f3460' stroke-width='3'/>
  <circle cx='400' cy='270' r='30' fill='#e94560'/>
  <text x='400' y='280' text-anchor='middle' fill='white' font-size='20' font-family='monospace'>&lt;/&gt;</text>
  <text x='400' y='520' text-anchor='middle' fill='#e94560' font-size='24' font-family='Arial'>💻 Технологии 💻</text>
</svg>";

        private static string GetArtSvg() => @"<svg xmlns='http://www.w3.org/2000/svg' width='800' height='600' viewBox='0 0 800 600'>
  <rect width='800' height='600' fill='#2c2c2c'/>
  <rect x='150' y='100' width='500' height='400' fill='white'/>
  <circle cx='400' cy='300' r='80' fill='#FF5722'/>
  <rect x='250' y='250' width='80' height='80' fill='#2196F3'/>
  <polygon points='550,220 600,300 500,300' fill='#4CAF50'/>
  <path d='M 200 150 Q 250 200 300 150' stroke='#9C27B0' stroke-width='5' fill='none'/>
  <text x='400' y='550' text-anchor='middle' fill='white' font-size='24' font-family='Arial'>🎨 Искусство 🎨</text>
</svg>";
    }
}