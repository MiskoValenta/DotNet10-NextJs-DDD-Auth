"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";

export default function RegisterPage() {
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState("");
    const [isLoading, setIsLoading] = useState(false);
    const router = useRouter();

    const handleRegister = async (e: React.FormEvent) => {
        e.preventDefault();
        setError("");

        // basic validation in frontendu
        if (password !== confirmPassword) {
            setError("passwords do not match");
            return;
        }

        setIsLoading(true);

        try {
            const res = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/auth/register`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                // credentials: "include" ensures that cookies sent back by the backend are stored
                credentials: "include",
                body: JSON.stringify({ email, password }),
            });

            if (res.ok) {
                // Backend He signed us up right away (set cookies), let's go to the dashboard.
                router.push("/dashboard");
            } else {
                const data = await res.json();
                setError(data.message || "Registration failed. This email may already exist.");
            }
        } catch (err) {
            setError("Unable to connect to the server");
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div style={{ maxWidth: "300px", margin: "100px auto", fontFamily: "sans-serif" }}>
            <h2>Vytvořit účet</h2>
            {error && <p style={{ color: "red", fontSize: "14px" }}>{error}</p>}

            <form onSubmit={handleRegister} style={{ display: "flex", flexDirection: "column", gap: "10px" }}>
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
                    placeholder="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                    style={{ padding: "8px" }}
                />
                <input
                    type="password"
                    placeholder="confirm password"
                    value={confirmPassword}
                    onChange={(e) => setConfirmPassword(e.target.value)}
                    required
                    style={{ padding: "8px" }}
                />
                <button
                    type="submit"
                    disabled={isLoading}
                    style={{
                        padding: "10px",
                        cursor: isLoading ? "not-allowed" : "pointer",
                        fontWeight: "bold",
                        backgroundColor: isLoading ? "#ccc" : "#0070f3",
                        color: isLoading ? "#666" : "white",
                        border: "none",
                        borderRadius: "4px"
                    }}
                >
                    {isLoading ? "Register..." : "sign up"}
                </button>
            </form>

            {/* Button back on Login */}
            <div style={{ marginTop: "25px", textAlign: "center", borderTop: "1px solid #eee", paddingTop: "15px" }}>
                <p style={{ fontSize: "14px", color: "#555", marginBottom: "10px" }}>
                    Už máš svůj účet?
                </p>
                <button
                    onClick={() => router.push("/")}
                    type="button"
                    style={{
                        width: "100%",
                        padding: "8px",
                        cursor: "pointer",
                        backgroundColor: "#fff",
                        border: "1px solid #ccc",
                        borderRadius: "4px"
                    }}
                >
                    Back to login
                </button>
            </div>
        </div>
    );
}