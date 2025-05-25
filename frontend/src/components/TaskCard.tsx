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
        <div className="card p-4 cursor-pointer group hover:shadow-lg transition-all duration-200 border border-gray-200 hover:border-primary-300" onClick={onClick}>
            {/* Header Section - Always present */}
            <div className="flex items-start justify-between mb-4">
                <h3 className="font-semibold text-gray-900 group-hover:text-primary-600 transition-colors duration-200 flex-1 pr-3">
                    {task.title}
                </h3>
                <span className={`px-2 py-1 rounded-full text-xs font-medium whitespace-nowrap ${currentStatus.color}`}>
                    {currentStatus.label}
                </span>
            </div>

            {/* Description Section - Conditional with consistent spacing */}
            <div className="mb-4 min-h-[2.5rem] flex items-start">
                {task.description ? (
                    <p className="text-gray-600 text-sm line-clamp-2 leading-relaxed">
                        {task.description}
                    </p>
                ) : (
                    <p className="text-gray-400 text-sm italic">
                        No description provided
                    </p>
                )}
            </div>

            {/* Footer Section - Always present with consistent height */}
            <div className="flex items-center justify-between min-h-[2rem]">
                {/* Due Date Section */}
                <div className="flex items-center text-sm">
                    {task.dueDate ? (
                        (() => {
                            const isCompleted = task.status === 3; // Completed status
                            const isDraft = task.status === 1; // Draft status
                            const isOverdue = new Date(task.dueDate) < new Date();
                            const isDueSoon = new Date(task.dueDate) <= new Date(Date.now() + 24 * 60 * 60 * 1000); // Due within 24 hours

                            // Keep gray color for completed and draft tasks
                            const getDateColor = () => {
                                if (isCompleted || isDraft) return 'text-gray-500';
                                if (isOverdue) return 'text-red-600';
                                if (isDueSoon) return 'text-orange-500';
                                return 'text-gray-500';
                            };

                            return (
                                <div className={`flex items-center ${getDateColor()}`}>
                                    <svg className="w-4 h-4 mr-1.5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                                    </svg>
                                    <span className={isOverdue && !isCompleted && !isDraft ? 'font-medium' : ''}>
                                        {formatDate(task.dueDate)}
                                        {isOverdue && !isCompleted && !isDraft && ' (Overdue)'}
                                        {isDueSoon && !isOverdue && !isCompleted && !isDraft && ' (Due Soon)'}
                                    </span>
                                </div>
                            );
                        })()
                    ) : (
                        <div className="flex items-center text-gray-400">
                            <svg className="w-4 h-4 mr-1.5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z" />
                            </svg>
                            <span className="italic">No due date</span>
                        </div>
                    )}
                </div>

                {/* Action Button */}
                <button
                    onClick={handleStatusClick}
                    className="btn-secondary py-1.5 px-3 text-sm hover:bg-primary-50 hover:text-primary-700 transition-colors duration-200 flex-shrink-0 ml-3"
                >
                    Mark as {nextStatus.label}
                </button>
            </div>
        </div>
    );
}