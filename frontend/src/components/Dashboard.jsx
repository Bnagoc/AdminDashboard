import { useEffect, useState } from 'react';
import api from '../services/Api';

export default function Dashboard() {
    const [clients, setClients] = useState([]);
    const [rate, setRate] = useState(null);
    const [newRate, setNewRate] = useState('');
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [payments, setPayments] = useState([]);

    useEffect(() => {
        fetchData();
    }, []);

    const fetchData = async () => {
        try {
            const rateResponse = await api.get('/rates');
            const paymentsResponse = await api.get('/payments?take=5');
            const clientsResponse = await api.get('/clients');

            setClients(clientsResponse.data.items);
            setRate(rateResponse.data.value);
            setPayments(paymentsResponse.data.items || []);
            setLoading(false);
        } catch (err) {
            setError('Не удалось загрузить данные');
            setLoading(false);
        }
    };

    const handleRateSubmit = async (e) => {
        e.preventDefault();

        try {
            const response = await api.post('/rates', { value: parseFloat(newRate) });
            setRate(response.data.value);
            setNewRate('');
        } catch (err) {
            setError('Не удалось обновить курс');
        }
    };

    if (loading) return <p>Загрузка...</p>;
    if (error) return <p style={{ color: 'red' }}>{error}</p>;

    return (
        
        <div className="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8">
            <div className="sm:mx-auto sm:w-full sm:max-w-xl">
                <h2 className="mt-10 text-center text-2xl font-bold tracking-tight text-gray-900">
                    Dashboard
                </h2>
            </div>

            <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-3xl">
                <h3 className="text-xl font-semibold mb-4">Клиенты</h3>
                <div className="overflow-x-auto">
                    <table className="min-w-full bg-white border border-gray-300 rounded-lg shadow-sm">
                        <thead className="bg-gray-100">
                            <tr>
                                <th className="py-2 px-4 border-b">Имя</th>
                                <th className="py-2 px-4 border-b">Email</th>
                                <th className="py-2 px-4 border-b">Баланс</th>
                            </tr>
                        </thead>
                        <tbody>
                            {clients.map(client => (
                                <tr key={client.id} className="hover:bg-gray-50">
                                    <td className="py-2 px-4 border-b">{client.name}</td>
                                    <td className="py-2 px-4 border-b">{client.email}</td>
                                    <td className="py-2 px-4 border-b">{client.balanceT || client.balance}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>
            </div>

            <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-md">
                <h3 className="text-xl font-semibold mb-4">Курс токенов</h3>
                <div className="border p-4 rounded-lg shadow-sm">
                    <p><strong>Текущий курс:</strong> {rate}</p>

                    <form onSubmit={handleRateSubmit} className="mt-4 space-y-2">
                        <label className="block text-sm font-medium text-gray-700">
                            Новый курс:
                            <input
                                type="number"
                                step="0.1"
                                min="0"
                                value={newRate}
                                onChange={(e) => setNewRate(e.target.value)}
                                className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
                                required
                            />
                        </label>
                        <button
                            type="submit"
                            className="w-full rounded-md bg-indigo-600 px-4 py-2 text-white hover:bg-indigo-700"
                        >
                            Обновить курс
                        </button>
                    </form>
                </div>
            </div>
            <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-3xl">
                <h3 className="text-xl font-semibold mb-4">История платежей</h3>
                <div className="overflow-x-auto">
                    <table className="min-w-full bg-white border border-gray-300 rounded-lg shadow-sm">
                        <thead className="bg-gray-100">
                            <tr>
                                <th className="py-2 px-4 border-b">ID</th>
                                <th className="py-2 px-4 border-b">Сумма</th>
                                <th className="py-2 px-4 border-b">Клиент</th>
                                <th className="py-2 px-4 border-b">Курс</th>
                                <th className="py-2 px-4 border-b">Дата</th>
                            </tr>
                        </thead>
                        <tbody>
                            {payments.map(payment => (
                                <tr key={payment.id} className="hover:bg-gray-50">
                                    <td className="py-2 px-4 border-b">{payment.id}</td>
                                    <td className="py-2 px-4 border-b">{payment.amount}</td>
                                    <td className="py-2 px-4 border-b">{payment.client}</td>
                                    <td className="py-2 px-4 border-b">{payment.rate}</td>
                                    <td className="py-2 px-4 border-b">
                                        {new Date(payment.createdAtUtc).toLocaleString()}
                                    </td>
                                </tr>
                            ))}
                            {payments.length === 0 && (
                                <tr>
                                    <td colSpan="5" className="py-4 text-center text-gray-500">
                                        Нет данных
                                    </td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </div>
            </div>

        </div>
    
    );
}