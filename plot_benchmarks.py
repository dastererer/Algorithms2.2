import csv
import math
import re
from collections import defaultdict
from pathlib import Path

import matplotlib.pyplot as plt


ROOT = Path(__file__).resolve().parent
COMMON_BENCH = ROOT.parent / "common" / "benchmarks" / "algorithms2_2"
LOCAL_BENCH = ROOT
BENCH_ROOT = COMMON_BENCH if COMMON_BENCH.exists() else LOCAL_BENCH
RESULTS_DIR = BENCH_ROOT / "BenchmarkDotNet.Artifacts" / "results"
OUTPUT_DIR = BENCH_ROOT / "graphs"


def parse_number(value):
    if value is None:
        return None

    text = str(value).strip().replace(",", ".")
    if not text or text == "NA":
        return None

    match = re.match(r"^([0-9]+(?:\.[0-9]+)?)\s*([a-zA-Zμ]*)$", text)
    if not match:
        return None

    number = float(match.group(1))
    unit = match.group(2)

    time_units = {
        "ns": 1,
        "us": 1_000,
        "μs": 1_000,
        "ms": 1_000_000,
        "s": 1_000_000_000,
    }

    memory_units = {
        "B": 1,
        "KB": 1024,
        "MB": 1024 * 1024,
        "GB": 1024 * 1024 * 1024,
    }

    if unit in time_units:
        return number * time_units[unit]

    if unit in memory_units:
        return number * memory_units[unit]

    if unit == "":
        return number

    return None


def read_csv_rows(path):
    with path.open("r", encoding="utf-8-sig", newline="") as file:
        return list(csv.DictReader(file))


def ensure_output_dir():
    OUTPUT_DIR.mkdir(exist_ok=True)


def save_plot(name):
    ensure_output_dir()
    plt.tight_layout()
    plt.savefig(OUTPUT_DIR / name, dpi=200)
    plt.close()


def plot_spell_checkers(path):
    rows = read_csv_rows(path)
    build_data = {}
    lookup_data = {}

    for row in rows:
        method = row["Method"]
        mean = parse_number(row.get("Mean"))
        if mean is None:
            continue

        if method.endswith("Build"):
            label = method.replace("Benchmark", "").replace("Build", "")
            build_data[label] = mean / 1_000_000
        else:
            label = method.replace("Benchmark", "")
            lookup_data[label] = mean / 1_000_000

    if build_data:
        plt.figure(figsize=(10, 6))
        plt.bar(build_data.keys(), build_data.values())
        plt.ylabel("Mean, ms")
        plt.title("Spell Checker Build Time")
        plt.xticks(rotation=25)
        save_plot("spell_checkers_build.png")

    if lookup_data:
        plt.figure(figsize=(10, 6))
        plt.bar(lookup_data.keys(), lookup_data.values())
        plt.ylabel("Mean, ms")
        plt.title("Spell Checker Lookup Time")
        plt.xticks(rotation=25)
        save_plot("spell_checkers_lookup.png")


def plot_hash_tables(path):
    rows = read_csv_rows(path)
    grouped = defaultdict(list)

    for row in rows:
        method = row["Method"]
        load_factor = parse_number(row.get("LoadFactor"))
        mean = parse_number(row.get("Mean"))
        if load_factor is None or mean is None:
            continue

        grouped[method].append((load_factor, mean))

    categories = {
        "search_success": ["SearchSuccess_Chaining", "SearchSuccess_Linear", "SearchSuccess_Double"],
        "search_fail": ["SearchFail_Chaining", "SearchFail_Linear", "SearchFail_Double"],
        "insert": ["Insert_Chaining", "Insert_Linear", "Insert_Double"],
    }

    titles = {
        "search_success": "Hash Tables Search Success",
        "search_fail": "Hash Tables Search Fail",
        "insert": "Hash Tables Insert",
    }

    for key, methods in categories.items():
        plt.figure(figsize=(10, 6))
        used = False
        for method in methods:
            if method not in grouped:
                continue

            points = sorted(grouped[method], key=lambda item: item[0])
            x_values = [point[0] for point in points]
            y_values = [point[1] for point in points]
            plt.plot(x_values, y_values, marker="o", label=method.replace("_", " "))
            used = True

        if used:
            plt.xlabel("Load Factor")
            plt.ylabel("Mean, ns")
            plt.title(titles[key])
            plt.legend()
            save_plot(f"hash_tables_{key}.png")
        else:
            plt.close()


def plot_dictionary_races(path):
    rows = read_csv_rows(path)
    time_grouped = defaultdict(list)
    memory_grouped = defaultdict(list)

    for row in rows:
        method = row["Method"]
        size = parse_number(row.get("DictionarySize"))
        mean = parse_number(row.get("Mean"))
        allocated = parse_number(row.get("Allocated"))
        if size is None:
            continue

        if mean is not None:
            time_grouped[method].append((size, mean))

        if allocated is not None:
            memory_grouped[method].append((size, allocated))

    build_methods = ["BuildRBTree", "BuildOrdinaryTrie", "BuildTernaryTrie", "BuildDAT"]
    lookup_methods = ["LookupRBTree", "LookupOrdinaryTrie", "LookupTernaryTrie", "LookupDAT"]

    plot_dictionary_lines(time_grouped, build_methods, "Dictionary Build Time", "Mean, ns", "dictionary_build_time.png")
    plot_dictionary_lines(time_grouped, lookup_methods, "Dictionary Lookup Time", "Mean, ns", "dictionary_lookup_time.png")
    plot_dictionary_lines(memory_grouped, build_methods, "Dictionary Build Memory", "Allocated, bytes", "dictionary_build_memory.png")


def plot_dictionary_lines(grouped, methods, title, ylabel, filename):
    plt.figure(figsize=(10, 6))
    used = False
    for method in methods:
        if method not in grouped:
            continue

        points = sorted(grouped[method], key=lambda item: item[0])
        x_values = [point[0] for point in points]
        y_values = [point[1] for point in points]
        plt.plot(x_values, y_values, marker="o", label=method)
        used = True

    if used:
        plt.xlabel("Dictionary Size")
        plt.ylabel(ylabel)
        plt.title(title)
        plt.legend()
        save_plot(filename)
    else:
        plt.close()


def main():
    if not RESULTS_DIR.exists():
        print("Results directory not found")
        return

    spell_checker_csv = RESULTS_DIR / "Algorithms.BenchmarkSpellCheckers-report.csv"
    hash_tables_csv = RESULTS_DIR / "Algorithms.BenchmarkHashTables-report.csv"
    dictionary_races_csv = RESULTS_DIR / "Algorithms.BenchmarkDictionaryRaces-report.csv"

    if spell_checker_csv.exists():
        plot_spell_checkers(spell_checker_csv)

    if hash_tables_csv.exists():
        plot_hash_tables(hash_tables_csv)

    if dictionary_races_csv.exists():
        plot_dictionary_races(dictionary_races_csv)

    generated = sorted(OUTPUT_DIR.glob("*.png")) if OUTPUT_DIR.exists() else []
    if not generated:
        print("No graphs generated")
        return

    for file in generated:
        print(file.name)


if __name__ == "__main__":
    main()