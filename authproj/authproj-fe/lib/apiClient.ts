const API_URL = process.env.NEXT_PUBLIC_API_URL;

export async function fetchWithAuth(endpoint: string, options: RequestInit = {}) {
    // Alwayes include cookies (credentials: include)
    const config: RequestInit = {
        ...options,
        credentials: "include",
        headers: {
            "Content-Type": "application/json",
            ...options.headers,
        },
    };

    let response = await fetch(`${API_URL}${endpoint}`, config);

    // If access token expired
    if (response.status === 401) {
        // Try calling refresh endpoint
        const refreshResponse = await fetch(`${API_URL}/auth/refresh`, {
            method: "POST",
            credentials: "include",
        });

        if (refreshResponse.ok) {
            // Refresh was successful (backend saved new cookies), we will repeat original question
            response = await fetch(`${API_URL}${endpoint}`, config);
        } else {
            // Refresh was unsuccessful (refresh token expired), redirect on login
            if (typeof window !== "undefined") {
                window.location.href = "/";
            }
        }
    }

    return response;
}