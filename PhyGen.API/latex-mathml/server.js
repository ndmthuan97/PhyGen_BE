import express from "express";
import cors from "cors";
import helmet from "helmet";
import katex from "katex";

const app = express();
app.disable("x-powered-by");
app.use(helmet());
app.use(cors(
    cors({
        origin: "*", // Cho tất cả
        methods: ["GET", "POST"],
        allowedHeaders: ["Content-Type"],
  }))
);
app.use(express.json({ limit: "1mb" }));

app.get("/healthz", (_, res) => res.status(200).send("ok"));

app.post("/latex-to-mathml", (req, res) => {
  try {
    const { latex, displayMode } = req.body || {};
    if (typeof latex !== "string" || latex.trim() === "") {
      return res.status(400).json({ error: "latex is required" });
    }
    if (latex.length > 20000) {
      return res.status(413).json({ error: "latex too large" });
    }

    const html = katex.renderToString(latex, {
      output: "mathml",
      displayMode: !!displayMode,
      throwOnError: false,
      strict: "ignore",
      trust: false, // đổi sang true/whitelist nếu cần
      macros: {
        // "\\vect": "\\mathbf{#1}"
      },
    });

    const match = html.match(/<math[\s\S]*?<\/math>/i);
    if (!match) {
      return res.status(422).json({ error: "Cannot extract <math> from KaTeX output" });
    }
    return res.json({ mathml: match[0] });
  } catch (e) {
    console.error("Render error:", e);
    return res.status(500).json({ error: e?.message || "Render error" });
  }
});

const PORT = process.env.PORT || 3101;
app.listen(PORT, () => console.log(`LaTeX→MathML service running on :${PORT}`));
