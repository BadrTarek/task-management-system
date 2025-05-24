const API_BASE_URL = 'http://localhost:5274/api';

export interface ApiResponse<T> {
    data?: T;
    error?: string;
}

export async function apiCall<T>(
    endpoint: string,
    options: RequestInit = {}
): Promise<ApiResponse<T>> {
    try {
        const response = await fetch(`${API_BASE_URL}${endpoint}`, {
            ...options,
            headers: {
                'Content-Type': 'application/json',
                ...options.headers,
            },
        });

        if (!response.ok) {
            throw new Error('API request failed');
        }

        const data = await response.json();
        return { data };
    } catch (error) {
        return { error: 'Something went wrong' };
    }
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface SignupRequest {
    name: string;
    email: string;
    password: string;
}

export interface AuthResponse {
    token: string;
    user: {
        id: string;
        name: string;
        email: string;
    };
}

export interface Task {
    id: string;
    title: string;
    description?: string;
    status: number;
    createdAt: string;
    dueDate?: string;
}

export interface CreateTaskRequest {
    title: string;
    description?: string;
    dueDate?: string;
}

export interface UpdateTaskRequest {
    title?: string;
    description?: string;
    dueDate?: string;
    status?: number;
}

export const authApi = {
    login: (data: LoginRequest) =>
        apiCall<AuthResponse>('/auth/login', {
            method: 'POST',
            body: JSON.stringify(data),
        }),

    signup: (data: SignupRequest) =>
        apiCall<AuthResponse>('/auth/signup', {
            method: 'POST',
            body: JSON.stringify(data),
        }),
};

export const taskApi = {
    getTasks: (token: string) =>
        apiCall<Task[]>('/tasks', {
            headers: { Authorization: `Bearer ${token}` },
        }),

    createTask: (token: string, data: CreateTaskRequest) =>
        apiCall<Task>('/tasks', {
            method: 'POST',
            body: JSON.stringify(data),
            headers: { Authorization: `Bearer ${token}` },
        }),

    updateTask: (token: string, id: string, data: UpdateTaskRequest) =>
        apiCall<Task>(`/tasks/${id}`, {
            method: 'PUT',
            body: JSON.stringify(data),
            headers: { Authorization: `Bearer ${token}` },
        }),
};