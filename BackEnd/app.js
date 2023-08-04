const express = require("express");
const app = express();

const PORT = 6161;

//InMemory DB
const players = {};

app.use(express.json());

//Funcs to easy test process :D
app.clearPlayers = () => {
  for (const player in players) {
    delete players[player];
  }
};

app.setPlayerScore = (player, score) => {
  players[player] = score;
};


//POST /score
/*
Receive
    {
        player: <PlayerName>,
        score: <PlayerScore>
    }
*/
app.post("/score", (req, res) => {
  const { player, score } = req.body;
  if (!player || !score || typeof score !== "number") {
    return res.status(400).json({ error: "Nome de jogador ou pontuação inválidos." });
  }

  players[player] = score;
  res.status(201).json({ message: "Player cadastrado com sucesso!" });
});

//GET /score/:<PlayerName>
/*
Returns
    {
        player: <PlayerName>,
        score: <PlayerScore>
    }
*/
app.get("/score/:player", (req, res) => {
  const player = req.params.player;
  const score = players[player];

  if (score === undefined) {
    return res.status(404).json({ error: "Player não encontrado." });
  }

  res.status(200).json({ player, score });
});

// GET /score
/*
Returns
    {
        player1: score1,
        player2: score2,
        ...
    }
*/
app.get("/score", (req, res) => {
    res.status(200).json(players);
  });

  app.get("/ranking", (req, res) => {
    const playerScores = Object.keys(players).map((player) => ({
      player,
      score: players[player],
    }));

    playerScores.sort((a, b) => b.score - a.score);
    const topThreePlayers = playerScores.slice(0, 3);
    res.status(200).json(topThreePlayers);
  });

app.listen(PORT, () => {
    console.log(`Servidor rodando na porta ${PORT}`);
  });

module.exports = app;