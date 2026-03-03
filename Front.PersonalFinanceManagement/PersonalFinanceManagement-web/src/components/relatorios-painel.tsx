import { useEffect, useState } from 'react'
import { ReportService, type ReportResultDto } from '../api-client'
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from './table'
import { Button } from './button'

export function RelatoriosPainel({ type }: { type: 'persons' | 'categories' }) {
  const [data, setData] = useState<ReportResultDto | null>(null)
  const [isLoading, setIsLoading] = useState(true)

  const fetchReport = async () => {
    setIsLoading(true)
    try {
      const response = type === 'persons' 
        ? await ReportService.getPersonsReport() 
        : await ReportService.getCategoriesReport()
      setData(response.data)
    } catch (error) {
      console.error('Erro ao buscar relatório:', error)
    } finally {
      setIsLoading(false)
    }
  }

  useEffect(() => {
    fetchReport()
  }, [type])

  return (
    <div className="flex flex-col gap-8">
      <div className="flex justify-between items-center">
        <h2 className="text-lg font-bold text-primary">
          {type === 'persons' ? 'Consolidado por Pessoa' : 'Consolidado por Categoria'}
        </h2>
        <Button 
          onClick={fetchReport} 
          className="bg-[#2b44a1] text-white hover:bg-[#1e327a] px-4 py-2 rounded-md font-medium"
        >
          Recarregar Dados
        </Button>
      </div>

      {isLoading ? (
        <p className="animate-pulse text-muted-foreground">Gerando cálculos...</p>
      ) : (
        <>
          {/* Cards com totais de receitas, despesas e geral */}
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            <div className="p-6 rounded-xl border border-border bg-surface shadow-sm">
              <p className="text-xs font-bold text-muted-foreground uppercase mb-1">Total Receitas</p>
              <p className="text-2xl font-black text-success">
                {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(data?.grandTotalIncome || 0)}
              </p>
            </div>
            <div className="p-6 rounded-xl border border-border bg-surface shadow-sm">
              <p className="text-xs font-bold text-muted-foreground uppercase mb-1">Total Despesas</p>
              <p className="text-2xl font-black text-destructive">
                {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(data?.grandTotalExpense || 0)}
              </p>
            </div>
            <div className="p-6 rounded-xl border border-primary/20 bg-primary/5 shadow-sm">
              <p className="text-xs font-bold text-primary uppercase mb-1">Saldo Geral</p>
              <p className={`text-2xl font-black ${(data?.grandTotalBalance || 0) >= 0 ? 'text-primary' : 'text-destructive'}`}>
                {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(data?.grandTotalBalance || 0)}
              </p>
            </div>
          </div>

          {/* Tabela */}
          <Table>
            <TableHeader>
              <TableRow>
                <TableHead>{type === 'persons' ? 'NOME' : 'DESCRIÇÃO'}</TableHead>
                <TableHead className="text-right">RECEITAS</TableHead>
                <TableHead className="text-right">DESPESAS</TableHead>
                <TableHead className="text-right">SALDO</TableHead>
              </TableRow>
            </TableHeader>
            <TableBody>
              {data?.items.map((item, idx) => (
                <TableRow key={idx}>
                  <TableCell className="font-bold">{item.nameOrDescription}</TableCell>
                  <TableCell className="text-right text-success">
                    {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(item.totalIncome)}
                  </TableCell>
                  <TableCell className="text-right text-destructive">
                    {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(item.totalExpense)}
                  </TableCell>
                  <TableCell className={`text-right font-black ${item.balance >= 0 ? 'text-primary' : 'text-destructive'}`}>
                    {new Intl.NumberFormat('pt-BR', { style: 'currency', currency: 'BRL' }).format(item.balance)}
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </>
      )}
    </div>
  )
}