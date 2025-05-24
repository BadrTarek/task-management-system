'use client';

import { Task, UpdateTaskRequest } from '@/utils/api';

interface TaskCardProps {
    task: Task;
    onStatusUpdate: (taskId: string, updateTaskRequest: UpdateTaskRequest) => void;
    onClick: () => void;
}

// To Do = 1,
// InProgress = 2,
// Completed = 3,
// Archived = 4,

const statusConfig = {
    1: { label: 'To Do', color: 'bg-gray-100 text-gray-800', nextStatus: 2 },
    2: { label: 'In Progress', color: 'bg-blue-100 text-blue-800', nextStatus: 3 },
    3: { label: 'Completed', color: 'bg-green-100 text-green-800', nextStatus: 4 },
    4: { label: 'Archived', color: 'bg-purple-100 text-purple-800', nextStatus: 1 },
};

export default function TaskCard({ task, onStatusUpdate, onClick }: TaskCardProps) {

    const currentStatus = statusConfig[task.status as keyof typeof statusConfig];
    const nextStatus = statusConfig[currentStatus.nextStatus as keyof typeof statusConfig];

    const formatDate = (dateString?: string) => {
        if (!dateString) return null;
        return new Date(dateString).toLocaleDateString('en-US', {
            month: 'short',
            day: 'numeric',
            year: 'numeric'
        });
    };

    const handleStatusClick = (e: React.MouseEvent) => {
        e.stopPropagation();
        onStatusUpdate(task.id, {
            status: currentStatus.nextStatus,
            title: task.title,
            description: task.description,
            dueDate: task.dueDate
        });
    };

    return (
        <div
            className="card p-4 cursor-pointer group"
            onClick={onClick}
        >
            <div className="flex items-start justify-between mb-3">
                <h3 className="font-semibold text-gray-900 group-hover:text-primary-600 transition-colors duration-200">
                    {task.title}
                </h3>
                <span className={`px-2 py-1 rounded-full text-xs font-medium ${currentStatus.color}`}>
                    {currentStatus.label}
                </span>
            </div>

            {task.description && (
                <p className="text-gray-600 text-sm mb-3 line-clamp-2">
                    {task.description}
                </p>
            )}

            <div className="flex items-center justify-between">
                {task.dueDate && (
                    <div className="flex items-center text-sm text-gray-500">
                        <svg className="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                        </svg>
                        {formatDate(task.dueDate)}
                    </div>
                )}

                <button
                    onClick={handleStatusClick}
                    className="btn-secondary py-1.5 px-3 text-sm"
                >
                    Mark as {nextStatus.label}
                </button>
            </div>
        </div>
    );
}