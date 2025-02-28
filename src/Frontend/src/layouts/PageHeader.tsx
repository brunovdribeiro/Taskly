interface PageHeaderProps {
    title: string;
    actionButton?: {
        label: string;
        onClick: () => void;
    };
}

export const PageHeader = ({ title, actionButton }: PageHeaderProps) => {
    return (
        <div className="bg-white shadow px-4 py-3 mb-4 flex justify-between items-center">
            <h1 className="text-xl font-semibold text-gray-800">{title}</h1>
            {actionButton && (
                <button
                    onClick={actionButton.onClick}
                    className="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded-md"
                >
                    {actionButton.label}
                </button>
            )}
        </div>
    );
};