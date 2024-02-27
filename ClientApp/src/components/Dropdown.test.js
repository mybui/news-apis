import React from "react";
import { render, screen } from "@testing-library/react";
import { act } from "react-dom/test-utils";
import { Dropdown } from "./Dropdown";

describe("<Dropdown />", () => {
  it("renders dropdown button correctly", () => {
    act(() => {
      render(
        <Dropdown
          label="testlabel"
          inputs={["Amazon", "Nestle", "Microsoft", "Konecranes"]}
          placeholder="Select"
        ></Dropdown>
      );
    });

    expect(screen.getByText("testlabel")).toBeInTheDocument();

    const dropdown = screen.getByRole("combobox");

    expect(dropdown).toBeInTheDocument();

    expect(screen.queryByText("Amazon")).not.toBeInTheDocument();
    expect(screen.queryByText("Nestle")).not.toBeInTheDocument();
    expect(screen.queryByText("Microsoft")).not.toBeInTheDocument();
    expect(screen.queryByText("Konecranes")).not.toBeInTheDocument();

    act(() => {
      dropdown.nextSibling.click();
    });
    expect(screen.getByText("Amazon")).toBeInTheDocument();
    expect(screen.getByText("Nestle")).toBeInTheDocument();
    expect(screen.getByText("Microsoft")).toBeInTheDocument();
    expect(screen.getByText("Konecranes")).toBeInTheDocument();
  });
});
