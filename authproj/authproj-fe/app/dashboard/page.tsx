"use client";

import { useEffect, useState } from "react";
import { fetchWithAuth } from "../../lib/apiClient";
import { useRouter } from "next/navigation";

export default function DashboardPage() {
    const [email, setEmail] = useState<string | null>(null);
    const router = useRouter();

    useEffect(() => {
        // We will use our wrapper, which solves 401 and possible auto-refresh
        fetchWithAuth("/auth/me")
            .then(async (res) => {
                if (!res.ok) throw new Error("unauthorized");
                const data = await res.json();
                setEmail(data.email);
            })
            .catch(() => {
                router.push("/");
            });
    }, [router]);

    const handleLogout = async () => {
        await fetchWithAuth("/auth/logout", { method: "POST" });
        router.push("/");
    };

    if (!email) return <p>Loading...</p>;

    return (
        <div style={{ maxWidth: "400px", margin: "100px auto", fontFamily: "sans-serif" }}>
            <h2>Welcome to Dashboardu</h2>
            <p>Loged in as: <strong>{email}</strong></p>
            <button onClick={handleLogout}>Log out</button>
            <p style={{ fontSize: "12px", color: "gray", marginTop: "20px" }}>
                Please wait for a moment (more than 1 minute). The Access Token will expire. After refreshing the page, you will see in the Network tab that a request to `/me` was first called (returns 401), the wrapper immediately calls `/refresh` (200), and then successfully repeats `/me` (200).
            </p>
        </div>
    );
}