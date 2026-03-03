import { twMerge } from 'tailwind-merge'
import type { ComponentProps } from 'react'

export function Table({ className, ...props }: ComponentProps<'table'>) {
  return (
    <div className="w-full overflow-auto rounded-lg border border-border bg-surface">
      <table data-slot="table" className={twMerge('w-full caption-bottom text-sm', className)} {...props} />
    </div>
  )
}

export function TableHeader({ className, ...props }: ComponentProps<'thead'>) {
  return <thead data-slot="table-header" className={twMerge('[&_tr]:border-b bg-primary text-primary-foreground', className)} {...props} />
}

export function TableBody({ className, ...props }: ComponentProps<'tbody'>) {
  return <tbody data-slot="table-body" className={twMerge('[&_tr:last-child]:border-0', className)} {...props} />
}

export function TableRow({ className, ...props }: ComponentProps<'tr'>) {
  return (
    <tr
      data-slot="table-row"
      className={twMerge('border-b border-border transition-colors hover:bg-muted/50 data-[state=selected]:bg-muted', className)}
      {...props}
    />
  )
}

export function TableHead({ className, ...props }: ComponentProps<'th'>) {
  return (
    <th
      data-slot="table-head"
      className={twMerge('h-12 px-4 text-left align-middle font-bold text-primary-foreground [&:has([role=checkbox])]:pr-0', className)}
      {...props}
    />
  )
}

export function TableCell({ className, ...props }: ComponentProps<'td'>) {
  return (
    <td
      data-slot="table-cell"
      className={twMerge('p-4 align-middle [&:has([role=checkbox])]:pr-0', className)}
      {...props}
    />
  )
}