import { useEffect, useState } from "react";
import { getAllOperations, type Operation } from "../../services/operationService";

export const OperationsExample = () => {
  const [operations, setOperations] = useState<Operation[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    // Fetch operations when component mounts
    const fetchOperations = async () => {
      try {
        setLoading(true);
        const response = await getAllOperations();
        
        if (response.success) {
          setOperations(response.data);
        } else {
          setError(response.message || "Error loading operations");
        }
      } catch (err) {
        setError(err instanceof Error ? err.message : "Unknown error");
      } finally {
        setLoading(false);
      }
    };

    fetchOperations();
  }, []);

  if (loading) return <div>Loading...</div>;
  if (error) return <div>Error: {error}</div>;

  return (
    <div className="p-4">
      <h2 className="text-2xl font-bold mb-4">Operations</h2>
      <div className="grid gap-4">
        {operations.map((operation) => (
          <div key={operation.id} className="border p-4 rounded-lg">
            <div className="flex items-center gap-2">
              <i className={operation.icon}></i>
              <h3 className="font-semibold">{operation.name}</h3>
            </div>
            <p className="text-sm text-gray-600">Route: {operation.route}</p>
            <p className="text-sm">
              Status: {operation.isActive ? "Active" : "Inactive"}
            </p>
          </div>
        ))}
      </div>
    </div>
  );
};
