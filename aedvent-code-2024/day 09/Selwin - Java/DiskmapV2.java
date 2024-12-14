package aoc_2024.day9;

import java.util.ArrayList;
import java.util.List;
import java.util.Optional;

public record DiskmapV2(List<AOCFile> files) {

    public long getResultPartB() {
        var compactedFiles = new ArrayList<>(files);
        for (int i = files.size()-1; i>0; i--) {
            var file = files.get(i);
            if (file.id() != null) {
                Optional<AOCFile> availableFileSpace = compactedFiles.stream()
                        .filter(f -> f.length() >= file.length() && f.id() == null).findFirst();

                if (availableFileSpace.isPresent()) {
                    var indexFreespace = compactedFiles.indexOf(availableFileSpace.get());
                    var fileToMoveIndex = compactedFiles.indexOf(file);
                    if (indexFreespace < fileToMoveIndex) {
                        compactedFiles.remove(file);
                        compactedFiles.add(fileToMoveIndex, new AOCFile(file.length(), null));
                        compactedFiles.remove(indexFreespace);
                        compactedFiles.add(indexFreespace, new AOCFile(file.length(), file.id()));
                        if (availableFileSpace.get().length() > file.length()) {
                            compactedFiles.add(indexFreespace+1, new AOCFile(availableFileSpace.get().length()-file.length(), null));
                        }
                    }
                }
            }
        }
        var index = 0L;
        var sum =0L;
        for (AOCFile file: compactedFiles) {
            for (int i =0; i< file.length(); i++) {
                if (file.id() != null) {
                    sum += file.id()*index;
                }
                index++;
            }
        }
        return sum;
    }
}
