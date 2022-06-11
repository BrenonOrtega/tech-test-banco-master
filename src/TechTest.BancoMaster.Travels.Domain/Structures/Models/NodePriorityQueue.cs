namespace TechTest.BancoMaster.Travels.Domain.Structures;

public class NodePriorityQueue : LinkedList<Node>
{
    public void AddNodeWithPriority(Node node)
    {
        if (this.Count == 0)
        {
            this.AddFirst(node);
        }
        else
        {
            if (node.Weight >= this.Last.Value.Weight)
            {
                this.AddLast(node);
            }
            else
            {
                for (LinkedListNode<Node> it = this.First; it != null; it = it.Next)
                {
                    if (node.Weight <= it.Value.Weight)
                    {
                        this.AddBefore(it, node);
                        break;
                    }
                }
            }
        }
    }

    public bool HasLetter(string letter)
    {
        for (LinkedListNode<Node> it = this.First; it != null; it = it.Next)
        {
            if (it.Value.Name == letter) { return true; }
        }
        return false;
    }
}
