"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";

export default function LoginPage() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const router = useRouter();

  const handleLogin = async (e: React.FormEvent) => {
    e.preventDefault();
    setError("");

    const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      credentials: "include",
      body: JSON.stringify({ email, password }),
    });

    if (res.ok) {
      router.push("/dashboard");
    } else {
      const data = await res.json();
      setError(data.message || "Login error");
    }
  };

  return (
    <div style={{ maxWidth: "300px", margin: "100px auto", fontFamily: "sans-serif" }}>
      <h2>Login</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}

      <form onSubmit={handleLogin} style={{ display: "flex", flexDirection: "column", gap: "10px" }}>
        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          required
          style={{ padding: "8px" }}
        />
        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          required
          style={{ padding: "8px" }}
        />
        <button type="submit" style={{ padding: "10px", cursor: "pointer", fontWeight: "bold" }}>
          Login
        </button>
      </form>

      {/* New section for redirecting on registration */}
      <div style={{ marginTop: "25px", textAlign: "center", borderTop: "1px solid #eee", paddingTop: "15px" }}>
        <p style={{ fontSize: "14px", color: "#555", marginBottom: "10px" }}>
          you don't have an account yet?
        </p>
        <button
          onClick={() => router.push("/register")}
          type="button"
          style={{
            width: "100%",
            padding: "8px",
            cursor: "pointer",
            backgroundColor: "#f5f5f5",
            border: "1px solid #ccc",
            borderRadius: "4px"
          }}
        >
          Create account
        </button>
      </div>
    </div>
  );
}