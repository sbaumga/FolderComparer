# FolderComparer
A simple tool to compare contents of folders that may be on different machines

## Instructions
To use this tool, first run the exe. When the program opens, you'll want to select a folder, select a file location, then click the button to output the contents of the folder to a text file.

To compare the contents of this file against another folder, select the folder you want to compare against, select the pre-existing file containing folder contents and then select a file location to output the comparison results to.

The comparisons are only done by partial file paths and modified dates, not by file contents. If the tool determines that the selected folder matches the selected file, not output file will be made. If there are discrepancies found, they will be listed in the output file. 

There are three types of discrepancies that can be found:  
1. NEW - File was not present in the selected file but is in the selected folder. Likely a file was added after the text file was generated.
2. MISSING - File was present in the selected file but is not in the selected folder. This could indicate a copying issue.
3. OUT OF DATE - File is present in both locations, but the version in the selected folder is older than the version the comparison file was made from.
