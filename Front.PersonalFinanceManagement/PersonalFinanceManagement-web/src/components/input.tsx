import { twMerge } from 'tailwind-merge'
import type { ComponentProps } from 'react'

export interface InputProps extends ComponentProps<'input'> {}

export function Input({ className, disabled, ...props }: InputProps) {
  return (
    <input
      data-slot="input"
      data-disabled={disabled ? '' : undefined}
      className={twMerge(
        'flex h-11 w-full rounded-md border border-input bg-surface px-4 py-2 text-sm text-foreground shadow-sm transition-colors',
        'file:border-0 file:bg-transparent file:text-sm file:font-medium',
        'placeholder:text-muted-foreground',
        'focus-visible:outline-none focus-visible:ring-2 focus-visible:ring-ring',
        'data-[disabled]:cursor-not-allowed data-[disabled]:opacity-50',
        className
      )}
      disabled={disabled}
      {...props}
    />
  )
}