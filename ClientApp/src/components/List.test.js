import { formatNewsItem, formatNews, timeAgo } from "./List";
import "@testing-library/jest-dom";
import React from "react";
import { render, waitFor } from "@testing-library/react";
import { List } from "./List";

describe("List Component", () => {
  beforeEach(() => {
    jest
      .spyOn(global, "fetch")
      .mockResolvedValue(
        new Response("Error with supplier news API response", { status: 500 })
      );
  });
  afterEach(() => {
    jest.clearAllMocks();
  });

  it("renders error message when API response is not OK", async () => {
    global.fetch.mockResolvedValueOnce({
      ok: false,
      json: jest.fn(),
    });
    const { getByText } = render(<List supplier="ExampleSupplier" />);
    await waitFor(() => {
      expect(
        getByText("Error with supplier news API response")
      ).toBeInTheDocument();
    });
  });

  it("renders news items when API response is OK", async () => {
    const mockData = [
      { id: 1, title: "News 1" },
      { id: 2, title: "News 2" },
    ];
    global.fetch.mockResolvedValueOnce({
      ok: true,
      json: () => Promise.resolve(mockData),
    });

    const { getByText } = render(<List supplier="ExampleSupplier" />);

    await waitFor(() => {
      expect(getByText("News 1")).toBeInTheDocument();
      expect(getByText("News 2")).toBeInTheDocument();
    });
  });

  it("handles load more button click", async () => {
    global.fetch
      .mockResolvedValueOnce({
        ok: true,
        json: () =>
          Promise.resolve([
            { id: 1, title: "News 1" },
            { id: 2, title: "News 2" },
          ]),
      })
      .mockResolvedValueOnce({
        ok: true,
        json: () =>
          Promise.resolve([
            { id: 3, title: "News 3" },
            { id: 4, title: "News 4" },
          ]),
      });

    const { getByText } = render(<List supplier="ExampleSupplier" />);
    await waitFor(() => {
      getByText("load more").click();
      expect(getByText("News 3")).toBeInTheDocument();
      expect(getByText("News 4")).toBeInTheDocument();
    });
  });

  describe("timeAgo", () => {
    it("should correctly format the time difference", async () => {
      const now = Date.now();
      expect(timeAgo(now)).toEqual("0 seconds ago");
      expect(timeAgo(now - 60000)).toEqual("1 minute ago");
      expect(timeAgo(now - 3600000)).toEqual("1 hour ago");
      expect(timeAgo(now - 86400000)).toEqual("1 day ago");
      expect(timeAgo(now - 604800000)).toMatch("7 days ago");
    });
  });

  describe("formatNewsItem", () => {
    it("should correctly format a news item", async () => {
      const item = {
        title: "Test News",
        snippet: "This is a test news snippet",
        publishedAt: new Date().toISOString(),
        url: "http://test.com",
      };
      const result = formatNewsItem(item);
      expect(result.props.children[0].props.href).toEqual(item.url);
      expect(result.props.children[0].props.children).toEqual(item.title);
      expect(result.props.children[1].props.children).toEqual(item.snippet);
      expect(result.props.children[2].props.children).toEqual("0 seconds ago");
    });
  });

  describe("formatNews", () => {
    it("should correctly format a list of news items", () => {
      const list = [
        {
          title: "Test News 1",
          snippet: "This is a test news snippet 1",
          publishedAt: new Date().toISOString(),
          url: "http://test1.com",
        },
        {
          title: "Test News 2",
          snippet: "This is a test news snippet 2",
          publishedAt: new Date().toISOString(),
          url: "http://test2.com",
        },
      ];
      const result = formatNews(list);
      expect(result.length).toEqual(2);
      expect(result[0].props.children[0].props.href).toEqual(list[0].url);
      expect(result[1].props.children[0].props.href).toEqual(list[1].url);
    });

    it("should return a message when the list is empty", () => {
      const result = formatNews([]);
      expect(result.props.children).toEqual(
        "No news items found for given supplier and country code"
      );
    });
  });
});
