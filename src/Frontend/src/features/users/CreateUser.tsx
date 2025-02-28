import { useState } from 'react';
import {CreateUserDto} from "../../api/generated/models.ts";

interface CreateUserFormProps {
    onSubmit: (user: CreateUserDto) => void;
    onCancel: () => void;
}

export const CreateUser = ({ onSubmit, onCancel }: CreateUserFormProps) => {
    const [formData, setFormData] = useState<CreateUserDto>({
        email: '',
        name: ''
    });

    const validateForm = (): boolean => { return true
    }

const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    if (validateForm()) {
        onSubmit(formData);
    }
};

return (
    <form onSubmit={handleSubmit} className="space-y-4">
        <div>
            <label htmlFor="name" className="block text-sm font-medium text-gray-700">
                Name
            </label>
            <input
                type="text"
                id="name"
                value={formData.name}
                onChange={(e) => setFormData({ ...formData, name: e.target.value })}
                className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 focus:border-blue-500 focus:outline-none"
            />
            {/*{errors.name && <p className="mt-1 text-sm text-red-600">{errors.name}</p>}*/}
        </div>
        <div>
            <label htmlFor="email" className="block text-sm font-medium text-gray-700">
                Email
            </label>
            <input
                type="email"
                id="email"
                value={formData.email}
                onChange={(e) => setFormData({ ...formData, email: e.target.value })}
                className="mt-1 block w-full rounded-md border border-gray-300 px-3 py-2 focus:border-blue-500 focus:outline-none"
            />
            {/*{errors.email && <p className="mt-1 text-sm text-red-600">{errors.email}</p>}*/}
        </div>
        <div className="flex justify-end space-x-3">
            <button
                type="button"
                onClick={onCancel}
                className="px-4 py-2 border border-gray-300 rounded-md text-gray-700 hover:bg-gray-50"
            >
                Cancel
            </button>
            <button
                type="submit"
                className="px-4 py-2 bg-blue-500 text-white rounded-md hover:bg-blue-600"
            >
                Save User
            </button>
        </div>
    </form>
)};