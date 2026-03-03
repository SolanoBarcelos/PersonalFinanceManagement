import { twMerge } from 'tailwind-merge'

interface TabBaseProps {
  children: any
  className?: string
}

export function Tabs({ children, className }: TabBaseProps) {
  return <div className={twMerge("flex flex-col w-full", className)}>{children}</div>
}

export function TabsList({ children, className }: TabBaseProps) {
  return (
    <div className={twMerge(
      "flex w-full items-stretch border-b-2 border-border", 
      className
    )}>
      {children}
    </div>
  )
}

export function TabsTab({ value, selectedValue, onClick, children, className }: any) {
  const isSelected = selectedValue === value
  return (
    <button
      type="button"
      onClick={onClick}
      className={twMerge(
        "flex-1 px-4 py-4 text-sm font-semibold transition-all cursor-pointer outline-none",
        "border-x border-t border-transparent -mb-[2px]",
        "text-muted-foreground hover:bg-muted/30",
        isSelected && "bg-surface text-primary border-border rounded-t-xl border-b-surface font-bold",
        className
      )}
    >
      {children}
    </button>
  )
}

export function TabsPanel({ value, selectedValue, children, className }: any) {
  if (selectedValue !== value) return null
  return (
    <div className={twMerge(
      "bg-surface p-8 border border-border border-t-0 rounded-b-xl shadow-sm",
      className
    )}>
      {children}
    </div>
  )
}