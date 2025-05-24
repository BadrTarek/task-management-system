'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/contexts/AuthContext';
import { taskApi, Task, CreateTaskRequest, UpdateTaskRequest } from '@/utils/api';
import TaskCard from '@/components/TaskCard';
import TaskModal from '@/components/TaskModal';
import ErrorPopup from '@/components/ErrorPopup';

export default function TasksPage() {
    const [tasks, setTasks] = useState<Task[]>([]);
    const [isLoading, setIsLoading] = useState(true);
    const [showModal, setShowModal] = useState(false);
    const [selectedTask, setSelectedTask] = useState<Task | undefined>();
    const [showError, setShowError] = useState(false);
    const [filter, setFilter] = useState<number | 'all'>('all');
    const { user, token, logout } = useAuth();
    const router = useRouter();

    useEffect(() => {
        if (!user) {
            router.push('/login');
            return;
        }
        loadTasks();
    }, [user, token, router]);

    const loadTasks = async () => {
        if (!token) return;

        const result = await taskApi.getTasks(token);
        if (result.data) {
            setTasks(result.data);
        } else {
            setShowError(true);
        }
        setIsLoading(false);
    };

    const handleCreateTask = async (data: CreateTaskRequest) => {
        if (!token) return;

        const result = await taskApi.createTask(token, data);
        if (result.data) {
            setTasks([result.data, ...tasks]);
        } else {
            setShowError(true);
        }
    };

    const handleUpdateTask = async (data: UpdateTaskRequest) => {
        if (!token || !selectedTask) return;

        const result = await taskApi.updateTask(token, selectedTask.id, data);
        if (result.data) {
            setTasks(tasks.map(task =>
                task.id === selectedTask.id ? result.data! : task
            ));
        } else {
            setShowError(true);
        }
    };

    const handleStatusUpdate = async (taskId: string, updateTaskRequest: UpdateTaskRequest) => {
        if (!token) return;

        const result = await taskApi.updateTask(token, taskId, updateTaskRequest);
        if (result.data) {
            setTasks(tasks.map(task =>
                task.id === taskId ? result.data! : task
            ));
        } else {
            setShowError(true);
        }
    };

    const handleTaskClick = (task: Task) => {
        setSelectedTask(task);
        setShowModal(true);
    };

    const handleNewTask = () => {
        setSelectedTask(undefined);
        setShowModal(true);
    };

    const handleModalSave = (data: CreateTaskRequest | UpdateTaskRequest) => {
        if (selectedTask) {
            handleUpdateTask(data as UpdateTaskRequest);
        } else {
            handleCreateTask(data as CreateTaskRequest);
        }
    };

    const filteredTasks = filter === 'all'
        ? tasks
        : tasks.filter(task => task.status === filter);

    const statusCounts = {
        1: tasks.filter(t => t.status === 1).length,
        2: tasks.filter(t => t.status === 2).length,
        3: tasks.filter(t => t.status === 3).length,
        4: tasks.filter(t => t.status === 4).length,
    };

    if (isLoading) {
        return (
            <div className="min-h-screen flex items-center justify-center">
                <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-primary-600"></div>
            </div>
        );
    }

    return (
        <div className="min-h-screen bg-gray-50">
            {/* Header */}
            <header className="bg-white shadow-sm border-b border-gray-200">
                <div className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
                    <div className="flex justify-between items-center h-16">
                        <div className="flex items-center">
                            <div className="w-8 h-8 bg-primary-600 rounded-lg flex items-center justify-center mr-3">
                                <svg className="w-5 h-5 text-white" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z" />
                                </svg>
                            </div>
                            <h1 className="text-xl font-semibold text-gray-900">Task Manager</h1>
                        </div>
                        <div className="flex items-center gap-4">
                            <span className="text-sm text-gray-600">Welcome, {user?.name}</span>
                            <button
                                onClick={logout}
                                className="text-sm text-gray-500 hover:text-gray-700"
                            >
                                Sign out
                            </button>
                        </div>
                    </div>
                </div>
            </header>

            {/* Main Content */}
            <main className="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 py-8">
                {/* Stats Cards */}
                <div className="grid grid-cols-2 md:grid-cols-4 gap-4 mb-8">
                    <div className="bg-white rounded-xl p-4 shadow-soft">
                        <div className="text-2xl font-bold text-gray-900">{statusCounts[1]}</div>
                        <div className="text-sm text-gray-600">To Do</div>
                    </div>
                    <div className="bg-white rounded-xl p-4 shadow-soft">
                        <div className="text-2xl font-bold text-blue-600">{statusCounts[2]}</div>
                        <div className="text-sm text-gray-600">In Progress</div>
                    </div>
                    <div className="bg-white rounded-xl p-4 shadow-soft">
                        <div className="text-2xl font-bold text-green-600">{statusCounts[3]}</div>
                        <div className="text-sm text-gray-600">Completed</div>
                    </div>
                    <div className="bg-white rounded-xl p-4 shadow-soft">
                        <div className="text-2xl font-bold text-purple-600">{statusCounts[4]}</div>
                        <div className="text-sm text-gray-600">Archived</div>
                    </div>
                </div>

                {/* Actions */}
                <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-4 mb-6">
                    <div className="flex flex-wrap gap-2">
                        <button
                            onClick={() => setFilter('all')}
                            className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${filter === 'all'
                                ? 'bg-primary-600 text-white'
                                : 'bg-white text-gray-700 hover:bg-gray-50'
                                }`}
                        >
                            All Tasks ({tasks.length})
                        </button>
                        <button
                            onClick={() => setFilter(1)}
                            className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${filter === 1
                                ? 'bg-primary-600 text-white'
                                : 'bg-white text-gray-700 hover:bg-gray-50'
                                }`}
                        >
                            To Do ({statusCounts[1]})
                        </button>
                        <button
                            onClick={() => setFilter(2)}
                            className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${filter === 2
                                ? 'bg-primary-600 text-white'
                                : 'bg-white text-gray-700 hover:bg-gray-50'
                                }`}
                        >
                            In Progress ({statusCounts[2]})
                        </button>
                        <button
                            onClick={() => setFilter(3)}
                            className={`px-4 py-2 rounded-lg text-sm font-medium transition-colors ${filter === 3
                                ? 'bg-primary-600 text-white'
                                : 'bg-white text-gray-700 hover:bg-gray-50'
                                }`}
                        >
                            Completed ({statusCounts[3]})
                        </button>
                    </div>
                    <button
                        onClick={handleNewTask}
                        className="btn-primary flex items-center gap-2"
                    >
                        <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 6v6m0 0v6m0-6h6m-6 0H6" />
                        </svg>
                        New Task
                    </button>
                </div>

                {/* Tasks Grid */}
                {filteredTasks.length === 0 ? (
                    <div className="text-center py-12">
                        <svg className="w-16 h-16 mx-auto text-gray-300 mb-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={1} d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
                        </svg>
                        <h3 className="text-lg font-medium text-gray-900 mb-2">No tasks found</h3>
                        <p className="text-gray-500 mb-6">
                            {filter === 'all' ? 'Get started by creating your first task' : 'No tasks match the current filter'}
                        </p>
                        {filter === 'all' && (
                            <button onClick={handleNewTask} className="btn-primary">
                                Create your first task
                            </button>
                        )}
                    </div>
                ) : (
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
                        {filteredTasks.map((task) => (
                            <TaskCard
                                key={task.id}
                                task={task}
                                onStatusUpdate={handleStatusUpdate}
                                onClick={() => handleTaskClick(task)}
                            />
                        ))}
                    </div>
                )}
            </main>

            {/* Modals */}
            <TaskModal
                isOpen={showModal}
                onClose={() => setShowModal(false)}
                task={selectedTask}
                onSave={handleModalSave}
            />

            <ErrorPopup
                isOpen={showError}
                onClose={() => setShowError(false)}
            />
        </div>
    );
}