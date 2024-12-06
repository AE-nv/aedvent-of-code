use regex::Regex;
use std::fs;

fn main() {
    let file_path = "input3.txt";
    let corrupted_memory = fs::read_to_string(file_path)
        .expect("Failed to read the input file");

    let instruction_re = Regex::new(r"(do\(\)|don't\(\)|mul\((\d+),(\d+)\))").unwrap();
    let mul_only_re = Regex::new(r"mul\((\d+),(\d+)\)").unwrap();

    let mut total_sum = 0;
    for capture in mul_only_re.captures_iter(&corrupted_memory) {
        let x: i32 = capture[1].parse().unwrap();
        let y: i32 = capture[2].parse().unwrap();
        total_sum += x * y;
    }
    println!("Part 1: {}", total_sum);

    let mut mul_enabled = true;
    total_sum = 0;
    for capture in instruction_re.captures_iter(&corrupted_memory) {
        if &capture[1] == "do()" {
            mul_enabled = true;
        } else if &capture[1] == "don't()" {
            mul_enabled = false;
        } else if let Some(x) = capture.get(2) {
            if mul_enabled {
                let x: i32 = x.as_str().parse().unwrap();
                let y: i32 = capture[3].parse().unwrap();
                total_sum += x * y;
            }
        }
    }
    println!("Part 2: {}", total_sum);
}