'use client';

import { useState, useEffect } from 'react';
import { Task, CreateTaskRequest, UpdateTaskRequest } from '@/utils/api';

interface TaskModalProps {
    isOpen: boolean;
    onClose: () => void;
    task?: Task;
    onSave: (data: CreateTaskRequest | UpdateTaskRequest) => void;
}

const statusOptions = [
    { value: 1, label: 'To Do', color: 'text-gray-800' },
    { value: 2, label: 'In Progress', color: 'text-blue-800' },
    { value: 3, label: 'Completed', color: 'text-green-800' },
    { value: 4, label: 'Archived', color: 'text-purple-800' },
];

export default function TaskModal({ isOpen, onClose, task, onSave }: TaskModalProps) {
    const [formData, setFormData] = useState({
        title: '',
        description: '',
        dueDate: '',
        status: 1,
    });

    useEffect(() => {
        if (task) {
            setFormData({
                title: task.title,
                description: task.description || '',
                dueDate: task.dueDate ? task.dueDate.split('T')[0] : '',
                status: task.status,
            });
        } else {
            setFormData({
                title: '',
                description: '',
                dueDate: '',
                status: 1,
            });
        }
    }, [task, isOpen]);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        if (!formData.title.trim()) return;

        const data: CreateTaskRequest | UpdateTaskRequest = {
            title: formData.title,
            description: formData.description || undefined,
            dueDate: formData.dueDate || undefined,
            ...(task && { status: formData.status }),
        };

        onSave(data);
        onClose();
    };

    const formatDate = (dateString?: string) => {
        if (!dateString) return null;
        return new Date(dateString).toLocaleDateString('en-US', {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric'
        });
    };

    if (!isOpen) return null;

    return (
        <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
            <div
                className="fixed inset-0 bg-black bg-opacity-25 backdrop-blur-sm"
                onClick={onClose}
            />
            <div className="relative bg-white rounded-xl shadow-xl max-w-md w-full max-h-[90vh] overflow-y-auto">
                <div className="sticky top-0 bg-white border-b border-gray-200 px-6 py-4 rounded-t-xl">
                    <div className="flex items-center justify-between">
                        <h2 className="text-lg font-semibold text-gray-900">
                            {task ? 'Task Details' : 'Create New Task'}
                        </h2>
                        <button
                            onClick={onClose}
                            className="text-gray-400 hover:text-gray-600 transition-colors"
                        >
                            <svg className="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                                <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M6 18L18 6M6 6l12 12" />
                            </svg>
                        </button>
                    </div>
                </div>

                <form onSubmit={handleSubmit} className="p-6">
                    <div className="space-y-4">
                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Title *
                            </label>
                            <input
                                type="text"
                                value={formData.title}
                                onChange={(e) => setFormData({ ...formData, title: e.target.value })}
                                className="input-field"
                                placeholder="Enter task title"
                                required
                            />
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Description
                            </label>
                            <textarea
                                value={formData.description}
                                onChange={(e) => setFormData({ ...formData, description: e.target.value })}
                                className="input-field resize-none"
                                rows={3}
                                placeholder="Enter task description"
                            />
                        </div>

                        <div>
                            <label className="block text-sm font-medium text-gray-700 mb-1">
                                Due Date
                            </label>
                            <input
                                type="date"
                                value={formData.dueDate}
                                onChange={(e) => setFormData({ ...formData, dueDate: e.target.value })}
                                className="input-field"
                            />
                        </div>

                        {task && (
                            <div>
                                <label className="block text-sm font-medium text-gray-700 mb-1">
                                    Status
                                </label>
                                <select
                                    value={formData.status}
                                    onChange={(e) => setFormData({ ...formData, status: Number(e.target.value) })}
                                    className="input-field"
                                >
                                    {statusOptions.map((option) => (
                                        <option key={option.value} value={option.value}>
                                            {option.label}
                                        </option>
                                    ))}
                                </select>
                            </div>
                        )}

                        {task && (
                            <div className="bg-gray-50 rounded-lg p-4 space-y-2">
                                <div className="text-sm">
                                    <span className="font-medium text-gray-700">Created:</span>
                                    <span className="text-gray-600 ml-2">
                                        {formatDate(task.createdAt)}
                                    </span>
                                </div>
                            </div>
                        )}
                    </div>

                    <div className="flex gap-3 mt-6 pt-4 border-t border-gray-200">
                        <button
                            type="button"
                            onClick={onClose}
                            className="btn-secondary flex-1"
                        >
                            Cancel
                        </button>
                        <button
                            type="submit"
                            className="btn-primary flex-1"
                        >
                            {task ? 'Update Task' : 'Create Task'}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}