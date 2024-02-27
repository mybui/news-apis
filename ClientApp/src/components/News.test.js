import "@testing-library/jest-dom";
import React from "react";
import { MemoryRouter } from "react-router-dom";
import { render, waitFor } from "@testing-library/react";
import App from "./../App";

describe("News Page", () => {
  beforeEach(() => {
    jest
      .spyOn(window, "fetch")
      .mockResolvedValue(new Response("Error", { status: 500 }));
  });

  afterEach(() => {
    jest.restoreAllMocks();
  });

  it("displays error message when supplier data fetch fails", async () => {
    const { getByText } = render(
      <MemoryRouter initialEntries={["/news"]}>
        <App />
      </MemoryRouter>
    );

    await waitFor(() => {
      expect(getByText("Error with supplier data")).toBeInTheDocument();
    });
  });

  it("returns error if suppliers not found", async () => {
    const { getByText } = render(
      <MemoryRouter initialEntries={["/news"]}>
        <App />
      </MemoryRouter>
    );
    await waitFor(() => {
      expect(getByText("Error with supplier data")).toBeInTheDocument();
    });
  });

  it("renders without crashing", async () => {
    render(
      <MemoryRouter initialEntries={["/news"]}>
        <App />
      </MemoryRouter>
    );
    // wait for fetch / useEffect to finish
    await waitFor(() => {});
  });

  describe("contents", () => {
    it("renders title", async () => {
      const { getByText } = render(
        <MemoryRouter initialEntries={["/news"]}>
          <App />
        </MemoryRouter>
      );

      await waitFor(() => {
        expect(getByText("News and alerts")).toBeInTheDocument();
      });
    });

    it("renders supplier and country selector titles", async () => {
      const { getByText } = render(
        <MemoryRouter initialEntries={["/news"]}>
          <App />
        </MemoryRouter>
      );
      await waitFor(() => {
        expect(getByText("Choose supplier")).toBeInTheDocument();
        expect(getByText("Country")).toBeInTheDocument();
      });
    });
  });
});
