using System.Collections.Generic;

public static class LocalizacaoDados
{
    private static readonly Dictionary<string, Dictionary<string, string>> textos = new Dictionary<string, Dictionary<string, string>>
    {
        { "jogar", new Dictionary<string, string>{ { "pt", "<b>JOGAR</b>" }, { "en", "<b>PLAY</b>" } } },
        { "como_jogar", new Dictionary<string, string>{ { "pt", "COMO JOGAR" }, { "en", "HOW TO PLAY" } } },
        { "creditos", new Dictionary<string, string>{ { "pt", "CRÉDITOS" }, { "en", "CREDITS" } } },
        { "sair", new Dictionary<string, string>{ { "pt", "SAIR" }, { "en", "QUIT" } } },
        { "fechar", new Dictionary<string, string>{ { "pt", "FECHAR" }, { "en", "CLOSE" } } },
        { "facil", new Dictionary<string, string>{ { "pt", "FÁCIL" }, { "en", "EASY" } } },
        { "medio", new Dictionary<string, string>{ { "pt", "MÉDIO" }, { "en", "MEDIUM" } } },
        { "dificil", new Dictionary<string, string>{ { "pt", "DIFÍCIL" }, { "en", "HARD" } } },
        { "escolha_dificuldade", new Dictionary<string, string>{ { "pt", "ESCOLHA A DIFICULDADE" }, { "en", "CHOOSE DIFFICULTY" } } },
        { "escolha_fase", new Dictionary<string, string>{ { "pt", "ESCOLHA A FASE" }, { "en", "CHOOSE LEVEL" } } },
        { "voltar", new Dictionary<string, string>{ { "pt", "VOLTAR" }, { "en", "BACK" } } },
        { "fase", new Dictionary<string, string>{ { "pt", "FASE" }, { "en", "LEVEL" } } },
        { "jogar_de_novo", new Dictionary<string, string>{ { "pt", "JOGAR DE NOVO" }, { "en", "PLAY AGAIN" } } },
        { "proxima_fase", new Dictionary<string, string>{ { "pt", "PRÓXIMA FASE" }, { "en", "NEXT LEVEL" } } },
        { "fase_concluida", new Dictionary<string, string>{ { "pt", "FASE CONCLUÍDA!" }, { "en", "LEVEL COMPLETE!" } } },
        { "tempo_esgotado", new Dictionary<string, string>{ { "pt", "TEMPO ESGOTADO!" }, { "en", "TIME'S UP!" } } },
        { "subtitulo", new Dictionary<string, string>{ { "pt", "Dois elementos. Um só caminho." }, { "en", "Two elements. One path." } } },
        { "como_jogar_titulo", new Dictionary<string, string>{ { "pt", "<b>Como Jogar</b>" }, { "en", "<b>How to Play</b>" } } },
        { "creditos_titulo", new Dictionary<string, string>{ { "pt", "<b>Créditos</b>" }, { "en", "<b>Credits</b>" } } },
        { "como_jogar_corpo", new Dictionary<string, string>{
            { "pt", "IGNIS (vermelho): Seta Esquerda / Seta Direita para mover, Seta Cima para pular.\n\nAQUA (azul): A / D para mover, W para pular.\n\nCada cristal só pode ser coletado pelo personagem do elemento correspondente. Cristais dourados podem ser coletados por qualquer um dos dois.\n\nIGNIS não pode tocar a água. AQUA não pode tocar a lava. Levem os dois até as portas do elemento certo para vencer a fase.\n\nNo modo Fácil não há tempo limite; nos modos Médio e Difícil, fiquem de olho no cronômetro!" },
            { "en", "IGNIS (red): Left Arrow / Right Arrow to move, Up Arrow to jump.\n\nAQUA (blue): A / D to move, W to jump.\n\nEach crystal can only be collected by the matching element's character. Golden crystals can be collected by either of them.\n\nIGNIS cannot touch water. AQUA cannot touch lava. Get both of them to the correct doors to complete the level.\n\nEasy mode has no time limit; Medium and Hard modes have a countdown, so keep an eye on the clock!" },
        } },
        { "creditos_corpo", new Dictionary<string, string>{
            { "pt", "Dual Elements\nUm jogo de plataforma cooperativo sobre fogo e água.\n\nArte: pacotes Kenney (kenney.nl), licença CC0.\nMotor: Unity.\n\nObrigado por jogar!" },
            { "en", "Dual Elements\nA cooperative platformer about fire and water.\n\nArt: Kenney asset packs (kenney.nl), CC0 license.\nEngine: Unity.\n\nThanks for playing!" },
        } },
    };

    public static string Get(string chave)
    {
        if (textos.TryGetValue(chave, out var porIdioma) && porIdioma.TryGetValue(Idioma.Atual, out var texto))
            return texto;
        return chave;
    }
}
