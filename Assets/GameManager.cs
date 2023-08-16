using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using DG.Tweening;
using Unity.VisualScripting;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;
using TMPro;


public class GameManager : MonoBehaviour
{
    public Disk diskPrefab;

    public static int n = 3;

    public Tower start;
    public Tower middle;
    public Tower end;
    public static float time = 0.2f;
    public TMP_InputField diskInput;
    public TMP_InputField timeInput;
    List<Disk> startDisk;
    List<Disk> midDisk;
    List<Disk> endDisk;
    bool started;


    List<Tower> results;

    // Start is called before the first frame update
    void Start()
    {
        started = false;
        diskInput.text = n.ToString();
        timeInput.text = time.ToString();
        for (int i = 0; i < n; i++)
        {
            Disk disk = Instantiate(diskPrefab, start.transform);

            disk.transform.localPosition = Vector3.up * (18 * (i + 1) + 9);
            disk.transform.localScale = new Vector3(1 - (float)(i + 1) / (n + 1), 1, 1);
            disk.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 1, 1 - (float)i / n);

            start.disks.Add(disk);

            
        }

        results = tower(n);
        string output = "";
        for (int i = 0; i < results.Count; i++)
        {
            output += results[i].name + " ";
        }
        print(output);
    }


    public void ReadInputDisk(string input)
    {
        try
        {
            int n_input = Int32.Parse(input);
            n = n_input <= 10 && n_input >= 1 ? n_input : 10;
        }
        catch (FormatException)
        {
            diskInput.text = n.ToString();

        }
    }

    public void ReadInputTime(string input)
    {
        try
        {
            float t = float.Parse(input);
            time = t >= 0 ? t : 10;
        }
        catch (FormatException)
        {
            timeInput.text = time.ToString();
        }
    }
    List<Tower> tower(int n)
    {
        if (n == 1)
        {
            return new List<Tower>() { start, end };
        }

        else
        {
            List<Tower> add = tower(n - 1);


            List<Tower> back = new List<Tower>(add.Count);
            List<Tower> front = new List<Tower>(add.Count);
            for (int i = 0; i < add.Count; i++)
            {
                if (add[i] == end) { back.Add(middle); }
                else if (add[i] == middle) { back.Add(end);}
                else { back.Add(add[i]); }

                if (add[i] == start) { front.Add(middle); }
                else if (add[i] == middle) { front.Add(start); }
                else { front.Add(add[i]); }
            }

            List<Tower> result = new List<Tower>(back.Count + 1 + front.Count);
            result.AddRange(back);
            result.AddRange(tower(1));
            result.AddRange(front);

            return result;
        }
    }



    public void OnPress()
    {
        if (!started)
        {
            StartCoroutine(Requests(results));
            started = true;
        }
            

    }

    public void Reset()
    {
        
        SceneManager.LoadScene(0);
    }
    IEnumerator Requests(List<Tower> req)
    {
        for (int i = 0; i < req.Count; i+= 2)
        {
            yield return StartCoroutine(MoveTo(req[i], req[i+1]));
        }
        
        
    }

    IEnumerator MoveTo(Tower from, Tower to)
    {
        Disk top = from.disks[from.disks.Count - 1];
        Transform T = top.transform;

        from.disks.Remove(top);
        to.disks.Add(top);

        yield return T.DOLocalMoveY(350, time).WaitForCompletion();
        yield return T.DOMoveX(to.transform.position.x, time).WaitForCompletion();

        T.SetParent(to.transform);

        yield return T.DOLocalMoveY(9 + to.disks.Count * 18, time).WaitForCompletion();
    }


}
