import React from "react";

import {
  Button,
  ComboBox,
  Input,
  Label,
  ListBox,
  ListBoxItem,
  Popover,
} from "react-aria-components";

import { cn } from "../utils";

/**
 * Sievo-styled dropdown component built with React-aria Combobox.
 *
 * @param {{
 *  label: string;
 *  inputs: string[];
 *  className?: string;
 *  inputClassName?: string;
 *  placeholder?: string;
 * } & import("react").ComponentPropsWithoutRef<typeof ComboBox>} props
 *
 * @see https://react-spectrum.adobe.com/react-aria/ComboBox.html
 */
export const Dropdown = ({
  label,
  inputs,
  className,
  inputClassName,
  placeholder,
  ...props
}) => {
  return (
    <ComboBox
      menuTrigger="focus"
      className={cn("flex flex-col gap-1", className)}
      {...props}
    >
      <Label
        style={{ fontSize: "16px" }}
        className={"heading-h5-bold text-struct-700 mb-2"}
      >
        {label}
      </Label>
      <div className="flex relative">
        <Input
          className={cn(
            "border border-struct-200 p-2 w-full label-1-regular placeholder:text-struct-200 h-8 rounded text-struct-900-sievo data-[focused]:outline-struct-700",
            inputClassName
          )}
          placeholder={placeholder}
        ></Input>
        <Button className={"absolute right-2 top-2"}>
          <span aria-hidden="true">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="16"
              height="16"
              viewBox="0 0 16 16"
              fill="none"
            >
              <path
                d="M11.5756 6H4.42445C4.25619 6 4.12999 6.08672 4.04586 6.26016C3.96173 6.4336 4.00379 6.60705 4.08792 6.73713L7.66348 11.1599C7.74761 11.2466 7.8738 11.3333 8 11.3333C8.1262 11.3333 8.25239 11.29 8.33652 11.1599L11.9121 6.73713C11.9962 6.60705 12.0383 6.4336 11.9541 6.26016C11.87 6.08672 11.7438 6 11.5756 6Z"
                fill="#1B324B"
              />
            </svg>
          </span>
        </Button>
      </div>
      <Popover
        className={
          "w-[var(--trigger-width)] border border-struct-200 rounded bg-white"
        }
      >
        <ListBox
          className={
            "max-h-[300px] overflow-auto w-[unset] min-h-[unset] block"
          }
        >
          {inputs
            .filter((i) => i !== "NO VALID NAME")
            .sort()
            .map((i) => (
              <ListBoxItem
                key={i}
                className={
                  "label-1-regular text-struct-900-sievo p-2 cursor-pointer data-[focused]:bg-struct-200"
                }
              >
                {i}
              </ListBoxItem>
            ))}
        </ListBox>
      </Popover>
    </ComboBox>
  );
};
