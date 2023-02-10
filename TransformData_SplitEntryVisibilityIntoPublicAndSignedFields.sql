UPDATE Entry
SET [Public] = 0, Signed = 0
WHERE Visibility = 0;

UPDATE Entry
SET [Public] = 0, Signed = 1
WHERE Visibility = 1;

UPDATE Entry
SET [Public] = 1, Signed = 1
WHERE Visibility = 2;